using UnityEngine;
using System;
using System.Collections.Generic;

public abstract class BaseContainer : MonoBehaviour
{
    [SerializeField] private Component[] _serializedComponents;

    private readonly Dictionary<Type, object> _components = new();

    protected virtual void Awake()
    {
        RegisterAllComponents();
    }

    protected void RegisterComponent(object component)
    {
        if (component == null) return;

        var type = component.GetType();
        if (!_components.ContainsKey(type))
            _components[type] = component;
    }

    private void RegisterAllComponents()
    {
        foreach (var root in _serializedComponents)
        {
            if (root == null) continue;
            
            IComponent[] components = root.GetComponentsInChildren<IComponent>(true);
            foreach (var component in components)
            {
                RegisterComponent(component);
            }
        }
    }

    public T Get<T>() where T : class
    {
        return _components.TryGetValue(typeof(T), out var component) ? component as T : null;
    }
}