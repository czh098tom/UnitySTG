using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using Latticework.UnityEngine.Utilities;

namespace UnitySTG.Abstractions
{
    internal class GizmoMeshProvider : Singleton<GizmoMeshProvider>
    {
        private static readonly int ELLIPSE_CUTS = 12;

        public Mesh Ellipse { get; private set; }
        public Mesh Rect { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            CreateEllipse();
            CreateRect();
        }

        private void CreateEllipse()
        {
            Ellipse = new Mesh();
            var vert = new Vector3[ELLIPSE_CUTS + 1];
            var colors = new Color[ELLIPSE_CUTS + 1];
            for (int i = 0; i < ELLIPSE_CUTS; i++)
            {
                var r = Mathf.PI * 2 / ELLIPSE_CUTS * i;
                vert[i] = new Vector3(Mathf.Cos(r), Mathf.Sin(r), 0);
                colors[i] = Color.white;
            }
            vert[ELLIPSE_CUTS] = Vector3.zero;
            colors[ELLIPSE_CUTS] = Color.white;
            Ellipse.SetVertices(vert);
            Ellipse.SetColors(colors);
            var tri = new int[ELLIPSE_CUTS * 3];
            for (int i = 0; i < ELLIPSE_CUTS; i++)
            {
                tri[i * 3] = 0;
                tri[i * 3 + 1] = (i + 1) % ELLIPSE_CUTS;
                tri[i * 3 + 2] = i;
            }
            Ellipse.SetTriangles(tri, 0);
            Ellipse.RecalculateNormals();
        }

        private void CreateRect()
        {
            Rect = new Mesh();
            var vert = new Vector3[4];
            var colors = new Color[4];
            for (int i = 0; i < 4; i++)
            {
                var r = Mathf.PI * 2 / 4;
                vert[i] = new Vector3(Mathf.Cos(r + Mathf.PI * 0.25f), Mathf.Sin(r + Mathf.PI * 0.25f), 0);
                colors[i] = Color.white;
            }
            Rect.SetVertices(vert);
            var tri = new int[6] { 0, 2, 1, 0, 3, 2 };
            Rect.SetTriangles(tri, 0);
            Rect.SetColors(colors);
            Ellipse.RecalculateNormals();
        }
    }
}
