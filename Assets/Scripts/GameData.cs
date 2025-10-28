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

    public float PlayerPositionX;
    public float PlayerPositionY;
    public float PlayerPositionZ;
    public float health;
    public float FlaskCount;

    public SettingsData settings;

    public List<EnemyData> enemies = new List<EnemyData>();

    public List<LootData> lootSouls = new List<LootData>();
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

[System.Serializable]
public class LootData
{
    public string lootID;
    public bool isCollected;
}
