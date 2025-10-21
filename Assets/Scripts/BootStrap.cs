using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.SceneManagement;

public class BootStrap
{
    private static BootStrap _instance;
    private Dictionary<Type, List<object>> _servises = new Dictionary<Type, List<object>>();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        SceneManager.sceneLoaded += InitializeScene;
        _instance = new BootStrap();
    }

    BootStrap()
    {
        RegisterInterfases();
        RegisterNotMonoBehObjects();
    }

    private static void InitializeScene(Scene scene, LoadSceneMode mode)
    {
        _instance.GetServises();
        _instance.ResolveAll<ISceneConfig>().FirstOrDefault(e => e._sceneName == scene.name).InitializeScene(_instance);
    }

    private void RegisterNotMonoBehObjects()
    {
        RegisterObject(new GameSettings());
        RegisterObject(new JsonSaveSystem());
        RegisterObject(new MenuesController());
        RegisterObject(new SavesManager());
        RegisterObject(new GamePlaySceneConfig());
        RegisterObject(new MainMenuSceneConfig());
    }

    private void RegisterInterfases()
    {
        _servises[typeof(ISaveable)] = new List<object>();
        _servises[typeof(IMenu)] = new List<object>();
        _servises[typeof(ISceneConfig)] = new List<object>();
    }

    private void RegisterObject(object obj)
    {
        var type = obj.GetType();

        if (!_servises.ContainsKey(type))
        {
            _servises[type] = new List<object>();
            _servises[type].Add(obj);
        }
        else
        {
            _servises[type].Add(obj);
        }

        foreach (var iface in type.GetInterfaces())
        {
            if (_servises.ContainsKey(iface))
                _servises[iface].Add(obj);
        }
    }

    public T Resolve<T>() where T : class
    {
        if (_servises.TryGetValue(typeof(T), out var list))
            return list.FirstOrDefault() as T;

        Debug.Log($"[BootStrap] service {typeof(T).Name} not found");
        return null;
    }

    public List<T> ResolveAll<T>() where T : class
    {
        if (_servises.TryGetValue(typeof(T), out var list))
            return list.Cast<T>().ToList();

        return new List<T>();
    }

    private void GetServises()
    {
        var behaivors = UnityEngine.Object.FindObjectsOfType<Behaviour>();

        foreach (var behaivor in behaivors)
        {
            var type = behaivor.GetType();

            if (!_servises.ContainsKey(type))
            {
                _servises[type] = new List<object>();
                _servises[type].Add(behaivor);
            }
            else
            {
                _servises[type].Add(behaivor);
            }

            foreach (var iface in type.GetInterfaces())
            {
                if (_servises.ContainsKey(iface))
                    _servises[iface].Add(behaivor);
            }
        }

        Debug.Log($"BootStrap registered {_servises.Count} types");
    }
}
