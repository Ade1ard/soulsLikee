using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BootStrap : MonoBehaviour
{
    private static BootStrap _instance;
    private Dictionary<Type, List<object>> _servises = new Dictionary<Type, List<object>>();

    private void Awake()
    {
        GetServises();
        RegisterNotMonoBehObjects();

        // initialization line

        Resolve<PlayerController>().Initialize(_instance);
        Resolve<PlayerHealth>().Initialize(_instance);
        Resolve<PlayerSword>().Initialize(_instance);

        Resolve<CameraModeChanger>().Initialize(_instance);

        Resolve<GameSettings>().Initialize(_instance);
        Resolve<GameSettingsView>().Initialize(_instance);

        Resolve<BonFireCont>().Initialize(_instance);
        Resolve<LevelUpCont>().Initialize(_instance);
        Resolve<Healing>().Initialize(_instance);
        Resolve<TutorialClueCont>().Initialize(_instance);
        Resolve<GetSoulsUI>().Initialize(_instance);

        foreach (LootSouls lootSouls in ResolveAll<LootSouls>())
            lootSouls.Initialize(_instance);
        foreach (EnemyController enemy in ResolveAll<EnemyController>())
            enemy.Initialize(_instance);
        foreach (EnemyCanvasLookAtCamera enemyCanvas in ResolveAll<EnemyCanvasLookAtCamera>())
            enemyCanvas.Initialize(_instance);
        foreach (EnemyHealth enemyHealth in ResolveAll<EnemyHealth>())
            enemyHealth.Initialize(_instance);
        foreach (EnemySword enemySword in ResolveAll<EnemySword>())
            enemySword.Initialize(_instance);

        Resolve<LootSpawner>().Initialize(_instance);
    }

    private void RegisterNotMonoBehObjects()
    {
        RegisterObject(new GameSettings());
        RegisterObject(new JsonSaveSystem());
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
    }

    public T Resolve<T>() where T : class
    {
        if (_servises.TryGetValue(typeof(T), out var list))
            return list.FirstOrDefault() as T;

        Debug.Log($"[BootStrap] —ервис {typeof(T).Name} не найден");
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
        _instance = this;

        var monoBehaivors = FindObjectsOfType<MonoBehaviour>(true);
        var behaivors = monoBehaivors.Concat(FindObjectsOfType<Behaviour>(true));

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
                {
                    _servises[type] = new List<object>();
                    _servises[type].Add(behaivor);
                }
            }
        }

        Debug.Log($"BootStrap зарегистрировал {_servises.Count} типов");
    }
}
