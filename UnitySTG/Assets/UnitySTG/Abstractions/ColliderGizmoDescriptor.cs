using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UnitySTG.Abstractions
{
    public class ColliderGizmoDescriptor : ScriptableObject
    {
        [Serializable]
        public class ColliderGizmoInfo
        {
            [field:SerializeField] public Color Color { get; private set; } = Color.white;
        }

        [field: SerializeField] 
        public SerializableDictionary<int, ColliderGizmoInfo> Info { get; private set; } = new();
    }
}
