﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnitySTG.Abstractions;

namespace UnitySTG.THSTG
{
    public class ComponentObject : LuaSTGObject
    {
        private readonly List<IComponent> _components = new();

        public ComponentObject(ILevelServiceProvider levelServiceProvider) : base(levelServiceProvider)
        {
        }

        public T AddComponent<T>() where T : IComponent, new()
        {
            var component = new T();
            _components.Add(component);
            component.LuaSTGObject = this;
            component.OnAttach();
            return component;
        }

        public void RemoveComponent(IComponent component)
        {
            _components.Remove(component);
        }

        public IComponent GetComponent(Type type)
        {
            for (int i = 0; i < _components.Count; i++)
            {
                if (_components[i].GetType() == type) return _components[i];
            }
            return null;
        }

        public IEnumerable<IComponent> GetComponents(Type type)
        {
            List<IComponent> components = new();
            for (int i = 0; i < _components.Count; i++)
            {
                if (_components[i].GetType() == type) components.Add(_components[i]);
            }
            return components;
        }

        protected override void OnColli(LuaSTGObject other)
        {
            for (int i = 0; i < _components.Count; i++)
            {
                _components[i].OnColli(other);
            }
            base.OnColli(other);
        }

        protected override void OnFrame()
        {
            for (int i = 0; i < _components.Count; i++)
            {
                _components[i].OnFrame();
            }
            base.OnFrame();
        }

        protected override void OnDestroy(DestroyEventArgs args)
        {
            for (int i = 0; i < _components.Count; i++)
            {
                _components[i].Dispose();
            }
            base.OnDestroy(args);
        }
    }
}
