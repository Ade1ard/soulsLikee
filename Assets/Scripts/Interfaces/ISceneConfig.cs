public interface ISceneConfig
{
    public string _sceneName {  get; }
    void InitializeScene(BootStrap bootStrap);
}