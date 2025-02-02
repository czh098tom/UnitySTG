using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitySTG.Abstractions
{
    public class ResourceDictionary : Dictionary<string, object>, IResourceDictionary
    {
        public static readonly ResourceDictionary Empty = new();

        public void MergeDictionary(IResourceDictionary other)
        {
            foreach (var kvp in other)
            {
                this[kvp.Key] = kvp.Value;
            }
        }
    }
}
