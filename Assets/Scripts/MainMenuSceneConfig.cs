public class MainMenuSceneConfig : ISceneConfig
{
    public string _sceneName { get; } = "MainMenuScene";
    public void InitializeScene(BootStrap bootStrap)
    {
        bootStrap.Resolve<MainMenu>().Initialize(bootStrap);
        bootStrap.Resolve<GameSettingsView>().Initialize(bootStrap);
    }
}
