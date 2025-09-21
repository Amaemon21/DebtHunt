using Zenject;

public class GameplayContainer : BaseContainer
{
    [Inject] private readonly InventoryService _inventoryService;
    
    protected override void Awake()
    {
        base.Awake();
        
        RegisterComponent(_inventoryService);
    }
}