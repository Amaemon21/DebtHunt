using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    [SerializeField] private GameplayConfig _gameplayConfig;
    [SerializeField] private GameplayContainer _gameplayContainer;
    
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<InputSystem>().AsSingle();
        Container.Bind<ScreenService>().AsSingle();
        
        Container.BindInterfacesAndSelfTo<InteractHandler>().AsSingle();
        
        Container.BindInstance(_gameplayContainer).AsSingle();
        Container.BindInterfacesAndSelfTo<GameplayProvider>().AsSingle();
        
        BindInventory();
    }

    private void BindInventory()
    {
        Container.BindInstance(_gameplayConfig).AsSingle();
        Container.BindInterfacesAndSelfTo<GameStatePlayerPrefsProvider>().AsSingle();
        Container.Bind<InventoryService>().AsSingle();
    }
}