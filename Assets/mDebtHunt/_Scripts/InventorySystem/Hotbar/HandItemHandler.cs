using System;
using R3;
using UnityEngine;
using Object = UnityEngine.Object;

public class HandItemHandler : IDisposable
{
    private readonly HotbarGrid _hotbarGrid;
    private readonly Transform _handTransform;

    private PickupbleInventoryItem _currentItem;
    private readonly CompositeDisposable _disposables = new();

    public HandItemHandler(HotbarGrid hotbarGrid, Transform handTransform)
    {
        _hotbarGrid = hotbarGrid;
        _handTransform = handTransform;

        hotbarGrid.ActiveIndex.Subscribe(_ => RefreshItem()).AddTo(_disposables);
        
        for (int i = 0; i < hotbarGrid.Capacity; i++)
        {
            int index = i;
            
            hotbarGrid.GetSlot(index).Item.Subscribe(_ => 
            {
                    if (_hotbarGrid.ActiveSlot() == index)
                        RefreshItem();
            }).AddTo(_disposables);
        }
    }

    private void RefreshItem()
    {
        ClearHand();

        var slot = _hotbarGrid.GetSlot(_hotbarGrid.ActiveSlot());
        var activeSlot = slot?.GetItem();

        if (activeSlot != null && activeSlot.ItemObject != null)
        {
            _currentItem = Object.Instantiate(activeSlot.ItemObject, _handTransform);
            _currentItem.Rigidbody.isKinematic = true;
            _currentItem.transform.localPosition = activeSlot.PositionHand;
            _currentItem.transform.localRotation = Quaternion.Euler(activeSlot.RotationHand);
        }
    }

    private void ClearHand()
    {
        if (_currentItem != null)
        {
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