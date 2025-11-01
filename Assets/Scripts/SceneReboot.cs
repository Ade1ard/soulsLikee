public class SceneReboot
{
    private BootStrap _bootStrap;

    public void Initialize(BootStrap bootStrap)
    {
        _bootStrap = bootStrap;
    }

    public void RebootScene()
    {
        foreach (IRebootable rebootable in _bootStrap.ResolveAll<IRebootable>())
            rebootable.Reboot();
    }
}
