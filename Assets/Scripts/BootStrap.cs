using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootStrap
{
    private static BootStrap _instance;
    private List<object> _globalServises = new List<object>();
    private Dictionary<Type, List<object>> _servises = new Dictionary<Type, List<object>>();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        SceneManager.sceneLoaded += InitializeScene;
        _instance = new BootStrap();
    }

    BootStrap()
    {
        RegisterNotMonoBehObjects();
    }

    private static void InitializeScene(Scene scene, LoadSceneMode mode)
    {
        _instance._servises.Clear();
        _instance.RegisterInterfases();
        _instance.RegisterGlobalObject();
        _instance.GetServises();
        _instance.ResolveAll<ISceneConfig>().FirstOrDefault(e => e._sceneName == scene.name).InitializeScene(_instance);
    }

    private void RegisterNotMonoBehObjects()
    {
        _globalServises.Add(new GameSettings());
        _globalServises.Add(new JsonSaveSystem());
        _globalServises.Add(new MenuesController());
        _globalServises.Add(new SavesManager());
        _globalServises.Add(new GamePlaySceneConfig());
        _globalServises.Add(new MainMenuSceneConfig());
        _globalServises.Add(new SceneReboot());
    }

    private void RegisterInterfases()
    {
        _servises[typeof(ISaveable)] = new List<object>();
        _servises[typeof(IMenu)] = new List<object>();
        _servises[typeof(ISceneConfig)] = new List<object>();
        _servises[typeof(IRebootable)] = new List<object>();
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
    private void RegisterGlobalObject()
    {
        foreach (var obj in _globalServises)
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
    }
}
