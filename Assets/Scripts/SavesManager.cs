using UnityEngine;

public class SavesManager
{
    private JsonSaveSystem _saveSystem;

    public void Initialize(BootStrap bootStrap)
    {
        _saveSystem = bootStrap.Resolve<JsonSaveSystem>();
    }

    public void LoadGame()
    {
        _saveSystem.LoadGame();
    }

    public void DeleteAllSaves()
    {
        foreach (var file in _saveSystem.GetSaveFiles())
            _saveSystem.DeleteSave(file);

        Debug.Log("Сохранения удалены");
    }
}
