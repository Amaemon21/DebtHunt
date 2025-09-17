using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller
{
    [SerializeField] private InventoryGridConfig _inventoryGridConfig;
    
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<InputSystem>().AsSingle();
        Container.Bind<ScreenService>().AsSingle();
        
        BindInventory();
    }

    private void BindInventory()
    {
        Container.BindInstance(_inventoryGridConfig).AsSingle();
        Container.BindInterfacesAndSelfTo<GameStatePlayerPrefsProvider>().AsSingle();
        Container.Bind<InventoryService>().AsSingle();
    }
}