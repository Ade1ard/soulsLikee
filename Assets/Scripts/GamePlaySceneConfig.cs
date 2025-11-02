public class GamePlaySceneConfig : ISceneConfig
{
    public string _sceneName { get; } = "GamePlayScene";
    public void InitializeScene(BootStrap bootStrap)
    {
        // initialization line

        bootStrap.Resolve<PlayerController>().Initialize(bootStrap);
        bootStrap.Resolve<PlayerSword>().Initialize(bootStrap);
        bootStrap.Resolve<Healing>().Initialize(bootStrap);
        bootStrap.Resolve<PlayerHealth>().Initialize(bootStrap);

        bootStrap.Resolve<CameraModeChanger>().Initialize(bootStrap);

        bootStrap.Resolve<GameSettings>().InitializeForGamePlayScene(bootStrap);
        bootStrap.Resolve<GameSettingsView>().Initialize(bootStrap);

        bootStrap.Resolve<BonFireCont>().Initialize(bootStrap);
        bootStrap.Resolve<BonFireMenu>().Initialize(bootStrap);

        bootStrap.Resolve<EscapeMenu>().Initialize(bootStrap);

        bootStrap.Resolve<MoneyCont>().Initialize(bootStrap);
        bootStrap.Resolve<LevelUpCont>().Initialize(bootStrap);

        bootStrap.Resolve<GetSoulsUI>().Initialize(bootStrap);
        bootStrap.Resolve<TutorialClueCont>().Initialize(bootStrap);

        foreach (LootSouls lootSouls in bootStrap.ResolveAll<LootSouls>())
            lootSouls.Initialize(bootStrap);
        foreach (EnemyController enemy in bootStrap.ResolveAll<EnemyController>())
            enemy.Initialize(bootStrap);
        foreach (EnemyCanvasLookAtCamera enemyCanvas in bootStrap.ResolveAll<EnemyCanvasLookAtCamera>())
            enemyCanvas.Initialize(bootStrap);
        foreach (EnemyHealth enemyHealth in bootStrap.ResolveAll<EnemyHealth>())
            enemyHealth.Initialize(bootStrap);
        foreach (EnemySword enemySword in bootStrap.ResolveAll<EnemySword>())
            enemySword.Initialize(bootStrap);
        foreach (DissolveController disCont in bootStrap.ResolveAll<DissolveController>())
            disCont.Initialize(bootStrap);

        bootStrap.Resolve<LootSpawner>().Initialize(bootStrap);
        bootStrap.Resolve<SceneReboot>().Initialize(bootStrap);
        bootStrap.Resolve<PlayerDeath>().Initialize(bootStrap);

        bootStrap.Resolve<JsonSaveSystem>().Initialize(bootStrap);
        bootStrap.Resolve<JsonSaveSystem>().LoadGame();
    }
}
