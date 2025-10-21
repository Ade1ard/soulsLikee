using System.IO;
using UnityEngine;
using System.Collections.Generic;

public class JsonSaveSystem
{
    private string savePath;
    private GameData currentGameData;
    private BootStrap _bootStrap;

    public static JsonSaveSystem Instance { get; private set; }

    public void Initialize(BootStrap bootStrap)
    {
        _bootStrap = bootStrap;

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
        foreach (ISaveable saveable in _bootStrap.ResolveAll<ISaveable>())
            saveable.SaveTo(currentGameData);

        string json = JsonUtility.ToJson(currentGameData, true);

        string filePath = GetSaveFilePath(saveName);
        File.WriteAllText(filePath, json);

        SaveBackup(saveName);

        Debug.Log($"Game saved: {filePath}");
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

                foreach (ISaveable saveable in _bootStrap.ResolveAll<ISaveable>())
                    saveable.LoadFrom(currentGameData);

                Debug.Log($"Game loaded: {filePath}");
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError($"load error: {e.Message}");
                return LoadBackup(saveName);
            }
        }
        else
        {
            currentGameData = new GameData();
            Debug.Log("Save not found, created new save");
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
            Debug.Log("Loading backup...");
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
}