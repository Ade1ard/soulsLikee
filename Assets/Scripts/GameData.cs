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
    public float health;

    public SettingsData settings;

    public List<EnemyData> enemies;
}

[System.Serializable]
public class EnemyData
{
    public string enemyID;
    public Vector3 enemyPosition;
    public bool isAlive;
    public float health;
}

[System.Serializable]
public class SettingsData
{
    public float UIsize;
    public float cameraDistanse;
    public float cameraSensity;
}
