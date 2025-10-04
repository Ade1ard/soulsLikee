using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int money;
    public int souls;

    public float maxHealth;
    public float damage;
    public float flaskEfficiency;
    public float oneUpgrateCost;

    public Vector3 playerPosition;

    public SettingsData settings;

    public List<EnemyData> enemies;

    public GameData()
    {
        playerPosition = new Vector3(26, 0, -70);
        money = 0;
        souls = 0;

        maxHealth = 100;
        damage = 30;
        flaskEfficiency = 50;
        oneUpgrateCost = 1200;
}
}

[System.Serializable]
public class EnemyData
{
    public Vector3 enemyPosition;
    public float health;

    public EnemyData()
    {
        health = 100;
        enemyPosition = Vector3.zero;
    }
}

[System.Serializable]
public class SettingsData
{
    public float UIsize;
    public float cameraDistanse;
    public float cameraSensity;

    public SettingsData()
    {
        UIsize = 0.5f;
        cameraDistanse = 0.5f;
        cameraSensity = 0.5f;
    }
}
