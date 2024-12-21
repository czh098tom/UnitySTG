using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics.FixedPoint;

namespace UnitySTG.Abstractions
{
    /// <summary>
    /// https://github.com/Legacy-LuaSTG-Engine/lstgx_Math/tree/d63939b5b5cd69ffeaa908f2ec38708a62ad3139
    /// </summary>
    internal class CollisionChecker
    {
        private static readonly fp sqrt3_2 = 0.8660254037844386467m;
        private static readonly fp sqrt3 = sqrt3_2 * 2;
        private static readonly fp sqrt2 = 1.414213562373095048801m;

        private static readonly fp pi_2 = fpmath.PI / 2m;
        private static readonly fp pi_4 = fpmath.PI / 4m;
        private static readonly fp pix2 = fpmath.PI * 2m;
        private static readonly fp pix4 = fpmath.PI * 4m;
        private static readonly fp pix2_3 = fpmath.PI * 2 / 3m;
        private static readonly fp pix4_3 = fpmath.PI * 4 / 3m;

        public bool CheckCollision(GameObjectController p1, GameObjectController p2)
        {
            var p1Cr = GetCR(p1);
            var p2Cr = GetCR(p2);
            //快速AABB检测
            if ((p1.X - p1Cr >= p2.X + p2Cr) ||
                (p1.X + p1Cr <= p2.X - p2Cr) ||
                (p1.Y - p1Cr >= p2.Y + p2Cr) ||
                (p1.Y + p1Cr <= p2.Y - p2Cr))
            {
                return false;
            }

            //外接圆碰撞检测，没发生碰撞则直接PASS
            if (CircleCircle(p1.X, p1.Y, p1Cr, p2.X, p2.Y, p2Cr))
            {
                return false;
            }

            return (p1.ColliderShape, p2.ColliderShape) switch
            {
                (ColliderShape.Circular, ColliderShape.Circular)
                    => EllipseEllipse(p1.X, p1.Y, p1.A, p1.B, p1.Rot, p2.X, p2.Y, p2.A, p2.B, p2.Rot),
                (ColliderShape.Rectangular, ColliderShape.Rectangular)
                    => ObbObb(p1.X, p1.Y, p1.A, p1.B, p1.Rot, p2.X, p2.Y, p2.A, p2.B, p2.Rot),
                (ColliderShape.Rectangular, ColliderShape.Circular)
                    => ObbEllipse(p1.X, p1.Y, p1.A, p1.B, p1.Rot, p2.X, p2.Y, p2.A, p2.B, p2.Rot),
                (ColliderShape.Circular, ColliderShape.Rectangular)
                    => ObbEllipse(p2.X, p2.Y, p2.A, p2.B, p2.Rot, p1.X, p1.Y, p1.A, p1.B, p1.Rot),
                (_, _) => false,
            };
        }

        private fp GetCR(GameObjectController p)
        {
            if (p.ColliderShape == ColliderShape.Rectangular)
            {
                //矩形
                return fpmath.sqrt(p.A * p.A + p.B * p.B);
            }
            else if (p.A != p.B)
            {
                //椭圆
                return p.A > p.B ? p.A : p.B;
            }
            else
            {
                //严格的正圆
                return p.A;
            }
        }

        readonly fp[] projOther = new fp[4];
        readonly fp2[] e = new fp2[4];
        private bool ObbObb(fp x0, fp y0, fp halfW0, fp halfH0, fp rot0,
            fp x1, fp y1, fp halfW1, fp halfH1, fp rot1)
        {
            SinCos(rot0, out var tSin0, out var tCos0);
            SinCos(rot1, out var tSin1, out var tCos1);
            e[0] = new(tCos0, tSin0);//e00
            e[1] = new(-tSin0, tCos0);//e01
            e[2] = new(tCos1, tSin1);//e10
            e[3] = new(-tSin1, tCos1);//e11
            projOther[0] = halfW0;
            projOther[1] = halfH0;
            projOther[2] = halfW1;
            projOther[3] = halfH1;
            var dx = x0 - x1;
            var dy = y0 - y1;
            for (byte i = 0; i < 4; i++)
            {
                var ii = 2 - i / 2 * 2;//2200
                var v0 = e[ii] * projOther[ii];
                var v1 = e[ii + 1] * projOther[ii + 1];
                var ex = e[i].x;
                var ey = e[i].y;
                var projHalfDiag = fpmath.max(
                    fpmath.abs(ex * (v0.x + v1.x) + ey * (v0.y + v1.y)),
                    fpmath.abs(ex * (v0.x - v1.x) + ey * (v0.y - v1.y))
                );
                if (projHalfDiag + projOther[i] < fpmath.abs(ex * dx + ey * dy))
                    return false;
            }
            return true;
        }

        private bool ObbEllipse(fp x0, fp y0, fp halfW, fp halfH, fp rot0,
            fp x1, fp y1, fp a, fp b, fp rot1)
        {
            if (a == b)
                return OBB_Circle(x0, y0, halfW, halfH, rot0, x1, y1, a);
            var p0 = new fp2(x0, y0);
            SinCos(rot0, out var tSin0, out var tCos0);
            SinCos(rot1, out var tSin1, out var tCos1);
            var e00 = new fp2(tCos0, tSin0);
            var e01 = new fp2(-tSin0, tCos0);
            var e11 = new fp2(-tSin1, tCos1);
            var f = e11 * (a / b - 1);
            var p0_ = p0 + PointLineSigned(x0, y0, x1, y1, e11) * f;
            var tmp = e00 * halfW + p0;
            var vDiag0 = tmp + e01 * halfH;
            var vDiag1 = tmp - e01 * halfH;
            var vDiag0_ = vDiag0 + PointLineSigned(vDiag0.x, vDiag0.y, x1, y1, e11) * f;
            var vDiag1_ = vDiag1 + PointLineSigned(vDiag1.x, vDiag1.y, x1, y1, e11) * f;
            var halfDiag0_ = vDiag0_ - p0_;
            var halfDiag1_ = vDiag1_ - p0_;
            var d = Point_Parallelogram(new fp2(x1, y1), p0_, halfDiag0_, halfDiag1_);
            return d <= a;
        }

        private bool OBB_Circle(fp x0, fp y0, fp halfW, fp halfH, fp rot,
            fp x1, fp y1, fp r)
        {
            SinCos(rot, out var tSin, out var tCos);
            var dx = x0 - x1;
            var dy = y0 - y1;
            var dw = fpmath.max(0m, fpmath.abs(tCos * dx + tSin * dy) - halfW);
            var dh = fpmath.max(0m, fpmath.abs(-tSin * dx + tCos * dy) - halfH);
            return r * r >= dh * dh + dw * dw;
        }

        private bool EllipseEllipse(
            fp x0, fp y0, fp a0, fp b0, fp rot0,
            fp x1, fp y1, fp a1, fp b1, fp rot1)
        {
            if (a0 == b0)
            {
                return CircleEllipse(x0, y0, a0, x1, y1, a1, b1, rot1);
            }
            if (a1 == b1)
            {
                return CircleEllipse(x1, y1, a1, x0, y0, a0, b0, rot0);
            }
            SinCos(rot1 - rot0, out var s, out var c);
            var c2 = c * c;
            var s2 = s * s;
            var sc = s * c;
            var a_ = 1 / (a1 * a1);
            var b_ = 1 / (b1 * b1);
            var m00 = (a_ * c2 + b_ * s2) * (a0 * a0);
            var m11 = (b_ * c2 + a_ * s2) * (b0 * b0);
            var m01 = (a_ - b_) * sc * (a0 * b0);
            var sum = m00 + m11;
            var tmp = m00 - m11;
            var dif = fpmath.sqrt(tmp * tmp + 4 * m01 * m01);
            var tanv = 2 * m01 / (dif + m00 - m11);
            SinCos(-rot0, out var s0, out var c0);
            var dx = x1 - x0;
            var dy = y1 - y0;
            return PointEllipseVal(
                0, 0, (dx * c0 - dy * s0) / a0, (dy * c0 + dx * s0) / b0,
                fpmath.sqrt(2 / (sum + dif)), fpmath.sqrt(2 / (sum - dif)), fpmath.atan(tanv)) <= 1;
        }

        private bool CircleEllipse(fp x0, fp y0, fp r, fp x1, fp y1, fp a, fp b, fp rot)
        {
            return PointEllipseVal(x0, y0, x1, y1, a, b, rot) <= r;
        }

        private fp PointEllipseVal(fp x0, fp y0,
            fp x1, fp y1, fp a, fp b, fp rotation)
        {
            if (a == b)
                return PointCircleVal(x0, y0, x1, y1, a);
            var px = x0 - x1;
            var py = y0 - y1;
            SinCos(-rotation, out var tSin, out var tCos);
            var x = fpmath.abs(px * tCos - py * tSin);
            var y = fpmath.abs(py * tCos + px * tSin);
            if (x * x / (a * a) + y * y / (b * b) <= 1.0m)
                return 0;
            var a2 = a * a;
            var b2 = b * b;
            var ax = a * x;
            var by = b * y;
            var tmp = b2 - a2;
            fp theta = pi_4 - (((b2 - a2) / sqrt2) + ax - by) / (ax + by);
            theta = fpmath.max(0, fpmath.min(theta, pi_2));
            SinCos(theta, out var st, out var ct);
            for (var i = 0; i < 2; ++i)
            {
                var dtheta =
                    (tmp * st * ct + ax * st - by * ct) /
                    (tmp * (ct * ct - st * st) + ax * ct + by * st);
                if (fpmath.abs(dtheta) < 1e-5m)
                    break;
                theta -= dtheta;
                SinCos(theta, out var st1, out var ct1);
                st = st1;
                ct = ct1;
            }
            var dx = a * ct - x;
            var dy = b * st - y;
            return fpmath.sqrt(dx * dx + dy * dy);
        }

        private fp PointCircleVal(fp x0, fp y0, fp x1, fp y1, fp r)
        {
            var dx = x1 - x0;
            var dy = y1 - y0;
            return fpmath.max(fpmath.sqrt(dx * dx + dy * dy) - r, 0);
        }

        private bool CircleCircle(fp x1, fp y1, fp r1, fp x2, fp y2, fp r2)
        {
            var sum = r1 + r2;
            var x = x2 - x1;
            var y = y2 - y1;
            return sum * sum >= x * x + y * y;
        }

        private fp Point_Parallelogram(fp2 p0,
            fp2 p1, fp2 A, fp2 B)
        {
            var p = p0 - p1;
            //var A = halfDiagA;
            //var B = halfDiagB;
            //var C = - A;
            //var D = - B;
            var AB = B - A;
            var AD = -B - A;
            var AX = p - A;
            var nA2B = fpmath.dot(AB, AX) < 0m;
            var nA2D = fpmath.dot(AD, AX) < 0m;
            if (nA2B && nA2D)
                return fpmath.length(AX);
            //var BA = -AB;
            //var& BC = AD;
            var BX = p - B;
            //var nB2A = BA.dot(BX) < 0m;
            var nB2A = fpmath.dot(AB, BX) > 0m;
            var nB2C = fpmath.dot(AD, BX) < 0m;
            if (nB2A && nB2C)
                return fpmath.length(BX);
            //var CB = -AD;
            //var& CD = BA;
            var CX = p + A;
            var nC2B = fpmath.dot(AD,CX) > 0m;
            //var nC2D = CD.dot(CX) < 0m;
            var nC2D = fpmath.dot(AB,CX) > 0m;
            if (nC2B && nC2D)
                return fpmath.length(CX);
            //var DA = -AD;
            //var& DC = AB;
            var DX = p + B;
            //var nD2A = DA.dot(DX) < 0m;
            var nD2A = fpmath.dot(AD,DX) > 0m;
            //var nD2C = DC.dot(DX) < 0m;
            var nD2C = fpmath.dot(AB,DX) < 0m;
            if (nD2A && nD2C)
                return fpmath.length(DX);

            if (!nA2B && !nB2A && CrossZ(AB, A) * CrossZ(AB, AX) > 0m)
            {
                var e = fpmath.normalize(AB);
                return PointLine(p, A, e.x, e.y);
            }
            if (!nC2D && !nD2C && CrossZ(AB, A) * CrossZ(AB, CX) < 0m)
            {
                var e = fpmath.normalize(AB);
                return PointLine(p, -A, e.x, e.y);
            }
            if (!nA2D && !nD2A && CrossZ(AD, A) * CrossZ(AD, AX) > 0m)
            {
                var e = fpmath.normalize(AD);
                return PointLine(p, A, e.x, e.y);
            }
            if (!nC2B && !nB2C && CrossZ(AD, A) * CrossZ(AD, BX) < 0m)
            {
                var e = fpmath.normalize(AD);
                return PointLine(p, B, e.x, e.y);
            }
            return 0m;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private fp CrossZ(fp2 v1, fp2 v2)
        {
            return v1.x * v2.y - v1.y * v2.x;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private fp PointLine(fp2 p0, fp2 p1, fp aCos, fp aSin)
        {
            return fpmath.abs(Point_Line_signed(p0, p1, aCos, aSin));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private fp PointLineSigned(fp x0, fp y0, fp x1, fp y1, fp2 e)
        {
            return e.x * (x0 - x1) + e.y * (y0 - y1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private fp Point_Line_signed(fp2 p0, fp2 p1, fp aCos, fp aSin)
        {
            return aSin * (p0.x - p1.x) - aCos * (p0.y - p1.y);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void SinCos(fp ang, out fp fSin, out fp fCos)
        {
            fSin = fpmath.sin(ang);
            fCos = fpmath.cos(ang);
        }
    }
}
