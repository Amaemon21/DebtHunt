using System;
using R3;
using UnityEngine;
using Object = UnityEngine.Object;

public class HandItemHandler : IDisposable
{
    private readonly HotbarGrid _hotbarGrid;
    private readonly InventoryService _inventoryService;
    private readonly Transform _handTransform;

    private PickupbleInventoryItem _currentItem;
    private readonly CompositeDisposable _disposables = new();

    public HandItemHandler(HotbarGrid hotbarGrid, InventoryService inventoryService, Transform handTransform)
    {
        _hotbarGrid = hotbarGrid;
        _inventoryService = inventoryService;
        _handTransform = handTransform;

        hotbarGrid.ActiveIndex.Subscribe(_ => CreateItem()).AddTo(_disposables);
        
        for (int i = 0; i < hotbarGrid.Capacity; i++)
        {
            int index = i;
            
            hotbarGrid.GetSlot(index).Item.Subscribe(_ => 
            {
                    if (_hotbarGrid.ActiveSlot() == index)
                        CreateItem();
            }).AddTo(_disposables);
        }
    }

    private void CreateItem()
    {
        ClearHand();
        
        InventorySlot slot = _hotbarGrid.GetSlot(_hotbarGrid.ActiveSlot());
        InventoryItemConfig activeSlot = slot?.Config;
        
        if (activeSlot != null && activeSlot.ItemObject != null)
        {
            _currentItem = Object.Instantiate(activeSlot.ItemObject, _handTransform);
            _currentItem.Rigidbody.isKinematic = true;
            _currentItem.transform.localPosition = activeSlot.PositionHand;
            _currentItem.transform.localRotation = Quaternion.Euler(activeSlot.RotationHand);
        }
    }
    
    public void DropItem()
    {
        if (_currentItem == null) 
            return;

        InventorySlot slot = _hotbarGrid.GetSlot(_hotbarGrid.ActiveSlot());
        var config = slot?.Config;
        
        if (config == null) 
            return;
        
        var droppedItem = _currentItem;
        _currentItem = null;

        droppedItem.transform.SetParent(null);
        droppedItem.Rigidbody.isKinematic = false;
        
        Transform cam = Camera.main.transform;
        droppedItem.transform.position = cam.position + cam.forward * 1f;
        droppedItem.transform.rotation = Quaternion.identity;
        
        droppedItem.Rigidbody.AddForce(cam.forward * 5f, ForceMode.Impulse);
        
        _inventoryService.RemoveItem(config, slot.Index);
    }

    private void ClearHand(bool destroy = true)
    {
        if (_currentItem != null)
        {
            if (destroy)
                Object.Destroy(_currentItem.gameObject);
        
            _currentItem = null;
        }
    }

    public void Dispose()
    {
        ClearHand();
        _disposables.Dispose();
    }
}