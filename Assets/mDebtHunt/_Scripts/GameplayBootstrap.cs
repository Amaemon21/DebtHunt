using UnityEngine;
using Zenject;

public class GameplayBootstrap : MonoBehaviour
{
    [Inject] private readonly GameStatePlayerPrefsProvider _gameStatePrefsProvider;
    [Inject] private readonly InventoryService _inventoryService;
    
    [SerializeField] private ItemsDatabase _itemsDatabase;
    [SerializeField] private HotbarController _hotbarController;
    
    private void Awake()
    {
        _gameStatePrefsProvider.Load();
        
        GameStateData gameState = _gameStatePrefsProvider.GameState;
        
        _inventoryService.RegisterInventory(gameState.Inventory, _itemsDatabase);
        
        _hotbarController.Setup();
    }
}