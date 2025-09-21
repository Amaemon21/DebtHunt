using System;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

public class InteractHandler : IInteractHandler, IInitializable, IDisposable
{
    private readonly InventoryService _inventoryService;
    private readonly InputSystem _inputSystem;

    private Camera _camera;
    private InteractProperty _property;
    
    private PickupbleInventoryItem _currentItem;
    
    public event Action OnItemPickupFailed;
    
    public InteractHandler(InventoryService inventoryService, InputSystem inputSystem)
    {
        _inventoryService = inventoryService;
        _inputSystem = inputSystem;
    }

    public void Initialize()
    {
        _inputSystem.InteractChanged += PickupItem;
    }
    
    public void SetProperty(InteractProperty property)
    {
        _property = property;
        _camera = _property.Camera;
    }

    public void Interactable()
    {
        Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f));

        if (Physics.Raycast(ray, out RaycastHit hit, _property.InteractRange, _property.HitScanMask))
        {
            if (hit.collider.TryGetComponent(out PickupbleInventoryItem item))
            {
                HandleItem(item);

                return;
            }
        }

        Clear();
    }
    
    private void HandleItem(PickupbleInventoryItem item)
    {
        if (_currentItem != item)
            Clear();

        _currentItem = item;

        ShowInteract(item);
    }
    
    private void PickupItem()
    {
        if (_currentItem != null)
        {
            AddItemsToInventoryGridResult result = _inventoryService.AddItem(_currentItem.InventoryItemConfig);

            if (result.ItemsAddedAmount > 0)
            {
                Object.Destroy(_currentItem.gameObject);
                Clear();
            }
            else
            {
                OnItemPickupFailed?.Invoke();
            }
        }
    }
    
    private void ShowInteract(PickupbleInventoryItem item)
    {
        if (item.Outline != null)
            item.Outline.enabled = true;
    }

    private void Clear()
    {
        if (_currentItem?.Outline != null)
            _currentItem.Outline.enabled = false;

        _currentItem = null;
    }

    public void Dispose()
    {
        _inputSystem.InteractChanged -= PickupItem;
    }
}