using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Latticework.UnityEngine.Utilities
{
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        private static T instance = null;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    var obj = FindFirstObjectByType<T>();
                    if (obj != null)
                    {
                        instance = obj;
                    }
                    else
                    {
                        instance = new GameObject(typeof(T).Name).AddComponent<T>();
                    }
                    DontDestroyOnLoad(instance);
                }
                return instance;
            }
        }

        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = (T)this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
