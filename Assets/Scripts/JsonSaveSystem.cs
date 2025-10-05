using System.IO;
using UnityEngine;
using System.Collections.Generic;

public class JsonSaveSystem : MonoBehaviour
{
    private string savePath;
    private GameData currentGameData;

    public static JsonSaveSystem Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeSaveSystem();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeSaveSystem()
    {
        savePath = Path.Combine(Application.persistentDataPath, "saves");

        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }

        LoadGame("autosave");
    }

    private string GetSaveFilePath(string saveName)
    {
        return Path.Combine(savePath, $"{saveName}.json");
    }

    public void SaveGame(string saveName = "autosave")
    {
        currentGameData = new GameData();

        CollectPlayerData();
        CollectCurrencyData();
        CollectEnemyData();
        CollectSettingsData();

        string json = JsonUtility.ToJson(currentGameData, true);

        string filePath = GetSaveFilePath(saveName);
        File.WriteAllText(filePath, json);

        SaveBackup(saveName);

        Debug.Log($"Игра сохранена: {filePath}");
    }

    public bool LoadGame(string saveName = "autosave")
    {
        string filePath = GetSaveFilePath(saveName);

        if (File.Exists(filePath))
        {
            try
            {
                string json = File.ReadAllText(filePath);
                currentGameData = JsonUtility.FromJson<GameData>(json);

                ApplyPlayerData();
                ApplyCurrencyData();
                ApplyEnemyData();
                ApplySettingsData();

                Debug.Log($"Игра загружена: {filePath}");
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Ошибка загрузки: {e.Message}");
                return LoadBackup(saveName);
            }
        }
        else
        {
            currentGameData = new GameData();
            Debug.Log("Сохранение не найдено, созданы новые данные");
            return false;
        }
    }

    private void CollectPlayerData()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            currentGameData.playerPosition = player.transform.position;
        }

        PlayerHealth playerStats = FindObjectOfType<PlayerHealth>();
        if (playerStats != null)
        {
            currentGameData.health = playerStats._value;
        }

        LevelUpCont level = FindObjectOfType<LevelUpCont>();
        if (level != null)
        {
            currentGameData.maxHealth = level._currentMaxHealth;
            currentGameData.damage = level._currentDamage;
            currentGameData.oneUpgrateCost = level._oneUpgrateCost;
            currentGameData.flaskEfficiency = level._currentFlaskEfficiency;

            currentGameData.souls = level._currentSoulsCount;
        }
    }

    private void CollectCurrencyData()
    {
        MoneyCont currencyManager = FindObjectOfType<MoneyCont>();
        if (currencyManager != null)
        {
            currentGameData.money = currencyManager._targetMoneyCount;
        }
    }

    private void CollectEnemyData()
    {
        currentGameData.enemies.Clear();

        EnemyController[] enemyObjects = EnemyController.FindObjectsOfType<EnemyController>();
        foreach (EnemyController enemy in enemyObjects)
        {
            if (enemy != null)
            {
                EnemyData enemyData = new EnemyData();
                enemyData.enemyID = enemy.name;
                enemyData.enemyPosition = enemy.transform.position;
                enemyData.isAlive = enemy.CheckAlive();
                enemyData.health = enemy.GetComponent<EnemyHealth>()._value;
                currentGameData.enemies.Add(enemyData);
            }
        }
    }

    private void CollectSettingsData()
    {
        GameSettings gameSettings = FindObjectOfType<GameSettings>();

        SettingsData settingsData = new SettingsData();
        settingsData.cameraDistanse = gameSettings._scrollbarCameraDist.value;
        settingsData.cameraSensity = gameSettings._scrollbarCameraSpeed.value;
        settingsData.UIsize = gameSettings._scrollbarUISize.value;

        currentGameData.settings = settingsData;
    }

    private void ApplyPlayerData()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.transform.position = currentGameData.playerPosition;
        }

        PlayerHealth playerStats = FindObjectOfType<PlayerHealth>();
        if (playerStats != null)
        {
            playerStats.DealDamage(playerStats._maxValue - currentGameData.health);
        }

        LevelUpCont level = FindObjectOfType<LevelUpCont>();
        if (level != null)
        {
            level.GetCurrienciesSouls(currentGameData.souls);

            level._currentMaxHealth = currentGameData.maxHealth;
            level._currentDamage = currentGameData.damage;
            level._currentFlaskEfficiency = currentGameData.flaskEfficiency;
            level._oneUpgrateCost = currentGameData.oneUpgrateCost;
        }
    }

    private void ApplyCurrencyData()
    {
        MoneyCont currencyManager = FindObjectOfType<MoneyCont>();
        if (currencyManager != null)
        {
            currencyManager.GetMoney(currentGameData.money);
        }
    }

    private void ApplyEnemyData()
    {
        foreach (EnemyData enemyData in currentGameData.enemies)
        {
            GameObject enemy = GameObject.Find(enemyData.enemyID);
            if (enemy != null)
            {
                EnemyController enemyController = enemy.GetComponent<EnemyController>();
                if (enemyController != null)
                {
                    enemyController.transform.position = enemyData.enemyPosition;
                    EnemyHealth enemyHealth = enemyController.GetComponent<EnemyHealth>();
                    if (enemyData.isAlive)
                    {
                        enemyHealth.TakeDamage(enemyHealth.maxValue - enemyData.health);
                    }
                    else
                    {
                        enemyHealth.EnemyDeath(false);
                    }
                }
            }
        }
    }

    private void ApplySettingsData()
    {
        GameSettings gameSettings = FindObjectOfType<GameSettings>();
        SettingsData settingsData = currentGameData.settings;
        if (gameSettings != null)
        {
            gameSettings._scrollbarCameraDist.value = settingsData.cameraDistanse;
            gameSettings.GetCameraDistanse();
            gameSettings._scrollbarCameraSpeed.value = settingsData.cameraSensity;
            gameSettings.GetCameraSpeed();
            gameSettings._scrollbarUISize.value = settingsData.UIsize;
            gameSettings.GetUISize();
        }
    }

    private void SaveBackup(string saveName)
    {
        string backupPath = GetSaveFilePath(saveName + "_backup");
        string originalPath = GetSaveFilePath(saveName);

        if (File.Exists(originalPath))
        {
            File.Copy(originalPath, backupPath, true);
        }
    }

    private bool LoadBackup(string saveName)
    {
        string backupPath = GetSaveFilePath(saveName + "_backup");

        if (File.Exists(backupPath))
        {
            Debug.Log("Загружаем резервную копию...");
            string json = File.ReadAllText(backupPath);
            currentGameData = JsonUtility.FromJson<GameData>(json);
            return true;
        }

        return false;
    }

    public List<string> GetSaveFiles()
    {
        List<string> saveFiles = new List<string>();

        if (Directory.Exists(savePath))
        {
            string[] files = Directory.GetFiles(savePath, "*.json");
            foreach (string file in files)
            {
                if (!file.EndsWith("_backup.json"))
                {
                    saveFiles.Add(Path.GetFileNameWithoutExtension(file));
                }
            }
        }

        return saveFiles;
    }

    public void DeleteSave(string saveName)
    {
        string filePath = GetSaveFilePath(saveName);
        string backupPath = GetSaveFilePath(saveName + "_backup");

        if (File.Exists(filePath))
            File.Delete(filePath);

        if (File.Exists(backupPath))
            File.Delete(backupPath);
    }

    private float lastAutoSaveTime;
    public float autoSaveInterval = 300f; // 5 минут

    void Update()
    {
        if (Time.time - lastAutoSaveTime > autoSaveInterval)
        {
            SaveGame("autosave");
            lastAutoSaveTime = Time.time;
        }
    }
}