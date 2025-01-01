using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitySTG.Abstractions
{
    public static class ServiceProviderExtension
    {
        public static T GetService<T>(this IServiceProvider serviceProvider)
            where T : class
        {
            return serviceProvider.GetService(typeof(T)) as T;
        }
    }
}
