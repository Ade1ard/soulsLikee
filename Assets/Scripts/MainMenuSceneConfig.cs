public class MainMenuSceneConfig : ISceneConfig
{
    public string _sceneName { get; } = "MainMenuScene";
    public void InitializeScene(BootStrap bootStrap)
    {
        bootStrap.Resolve<MainMenu>().Initialize(bootStrap);
        bootStrap.Resolve<GameSettingsView>().Initialize(bootStrap);
        bootStrap.Resolve<GameSettings>().InitializeForMainMenuScene(bootStrap);
        bootStrap.Resolve<NewGameMenu>().Initialize(bootStrap);
        bootStrap.Resolve<JsonSaveSystem>().Initialize(bootStrap);
        bootStrap.Resolve<SavesManager>().Initialize(bootStrap);
    }
}
