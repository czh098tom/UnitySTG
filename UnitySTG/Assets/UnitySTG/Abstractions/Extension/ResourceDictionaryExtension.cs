using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitySTG.Abstractions.Extension
{
    public static class ResourceDictionaryExtension
    {
        public static T GetResource<T>(this IResourceDictionary dict, string name) where T : class
        {
            return dict[name] as T;
        }
    }
}
