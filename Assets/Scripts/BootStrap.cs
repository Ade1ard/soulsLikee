using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BootStrap : MonoBehaviour
{
    private static BootStrap _instance;
    private Dictionary<Type, List<object>> _servises = new Dictionary<Type, List<object>>();

    private void Start()
    {
        // порядок инициализации


    }

    private void Awake()
    {
        _instance = this;

        var behaivors = FindObjectsOfType<MonoBehaviour>(true);
        foreach (var behaivor in behaivors)
        {
            var type = behaivor.GetType();

            if (!_servises.ContainsKey(type))
            {
                _servises[type] = new List<object>();
                _servises[type].Add(behaivor);
                Debug.Log($"зарегистрирован {type}");
                Debug.Log(behaivor);
            }

            foreach (var iface in type.GetInterfaces())
            {
                if (_servises.ContainsKey(iface))
                {
                    _servises[type] = new List<object>();
                    _servises[type].Add(behaivor);
                }
            }
        }

        Debug.Log($"BootStrap зарегистрировал {_servises.Count} типов");
    }

    public T Resolve<T>() where T : class
    {
        if (_servises.TryGetValue(typeof(T), out var list))
            return list.FirstOrDefault() as T;

        Debug.Log($"[BootStrap] Сервис {typeof(T).Name} не найден");
        return null;
    }

    public List<T> ResolveAll<T>() where T : class
    {
        if (_servises.TryGetValue(typeof(T), out var list))
            return list.Cast<T>().ToList();

        return new List<T>();
    }
}
