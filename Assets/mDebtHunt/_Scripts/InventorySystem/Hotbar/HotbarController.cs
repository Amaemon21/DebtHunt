using UnityEngine;
using Zenject;

public class HotbarController : MonoBehaviour, IComponent
{
    [Inject] private readonly GameplayProvider _gameplayProvider;
    [Inject] private readonly InputSystem _inputSystem;
    [Inject] private readonly InventoryService _inventoryService;
    
    [SerializeField] private HotbarView _hotbarView;
    
    [SerializeField] private Transform _handTransform;
    
    private int _capacity;
    private int _currentIndex = 0;
    private HotbarGrid _hotbarGrid;
    private HandItemHandler _handItemHandler;
    
    public int CurrentIndex => _currentIndex;

    public void Setup()
    {
        _capacity = _inventoryService.GetСapacity();
        _hotbarGrid = new HotbarGrid(_capacity);
        _hotbarView.Init(_hotbarGrid);
        
        for (int i = 0; i < _hotbarGrid.Capacity; i++)
        {
            _hotbarGrid.AssignSlot(i, _inventoryService.GetSlots()[i]);
        }
        
        _hotbarView.Refresh();

        _handItemHandler = new HandItemHandler(_hotbarGrid, _inventoryService, _handTransform);
        
        _inventoryService.OnInventoryChanged += _hotbarView.Refresh;
    }
    
    private void OnEnable()
    {
        _inputSystem.UseChanged += UseItem;
        _inputSystem.DropChanged += DropItem;
    }
    
    private void Update()
    {
        float scroll = _inputSystem.HotbarScroll;

        if (scroll > 0f)
        {
            _currentIndex--;
            
            if (_currentIndex < 0)
                _currentIndex = _capacity - 1;
            
            OnHotbarChanged();
        }
        else if (scroll < 0f)
        {
            _currentIndex++;
            
            if (_currentIndex >= _capacity)
                _currentIndex = 0;
            
            OnHotbarChanged();
        }
    }

    private void UseItem()
    {
        InventoryItemConfig currentItem = _hotbarGrid.GetSlot(_currentIndex).Config;

        if (currentItem != null)
        {
            currentItem.Use(_gameplayProvider);
        }
    }

    private void DropItem()
    {
        _handItemHandler.DropItem();
    }
    
    private void OnHotbarChanged()
    {
        _hotbarView.SetActive(_currentIndex);
    }
    
    private void OnDisable()
    {
        _inventoryService.OnInventoryChanged -= _hotbarView.Refresh;
        
        _inputSystem.UseChanged -= UseItem;
        _inputSystem.DropChanged -= DropItem;
        
        _handItemHandler.Dispose();
    }
}