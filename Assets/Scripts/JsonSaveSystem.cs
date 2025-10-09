using System.IO;
using UnityEngine;
using System.Collections.Generic;

public class JsonSaveSystem
{
    private string savePath;
    private GameData currentGameData;

    public static JsonSaveSystem Instance { get; private set; }

    public void Initialize()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializeSaveSystem();
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