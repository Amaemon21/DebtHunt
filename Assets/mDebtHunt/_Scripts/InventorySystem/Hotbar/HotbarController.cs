using UnityEngine;
using Zenject;

public class HotbarController : MonoBehaviour
{
    [Inject] private readonly InputSystem _inputSystem;
    [Inject] private readonly InventoryService _inventoryService;
    
    [SerializeField] private HotbarView _hotbarView;
    [SerializeField] private int slotsCount = 6;
    
    [SerializeField] private Transform _handTransform;
    
    private int currentIndex = 0;
    private HotbarGrid _hotbarGrid;
    private HandItemHandler _handItemHandler;

    public void Setup()
    {
        _hotbarGrid = new HotbarGrid(slotsCount);
        _hotbarView.Init(_hotbarGrid);
        
        for (int i = 0; i < _hotbarGrid.Capacity; i++)
        {
            _hotbarGrid.AssignSlot(i, _inventoryService.GetSlots()[i]);
        }
        
        _hotbarView.Refresh();

        _handItemHandler = new HandItemHandler(_hotbarGrid, _handTransform);
    }
    
    private void OnEnable()
    {
        _inventoryService.OnInventoryChanged += _hotbarView.Refresh;
    }

    private void OnDisable()
    {
        _inventoryService.OnInventoryChanged -= _hotbarView.Refresh;
        
        _handItemHandler.Dispose();
    }
    
    private void Update()
    {
        float scroll = _inputSystem.HotbarScroll;

        if (scroll > 0f)
        {
            currentIndex--;
            
            if (currentIndex < 0)
                currentIndex = slotsCount - 1;
            
            OnHotbarChanged();
        }
        else if (scroll < 0f)
        {
            currentIndex++;
            
            if (currentIndex >= slotsCount)
                currentIndex = 0;
            
            OnHotbarChanged();
        }
    }

    private void UseItem()
    {
        InventoryItemConfig currentItem = _hotbarGrid.GetSlot(currentIndex).GetItem();
        
        currentItem.Use();
    }

    private void OnHotbarChanged()
    {
        _hotbarView.SetActive(currentIndex);
    }
}