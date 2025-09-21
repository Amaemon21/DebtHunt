using System;
using System.Collections.Generic;

public abstract class BaseProvider
{
    // Теперь храним любые объекты, не только Component
    protected readonly Dictionary<Type, object> _components = new();

    protected void Add<T>(BaseContainer container) where T : class
    {
        var instance = container.Get<T>();
        if (instance != null)
        {
            _components[typeof(T)] = instance;
        }
    }

    public T Get<T>() where T : class
    {
        _components.TryGetValue(typeof(T), out var component);
        return component as T;
    }
}