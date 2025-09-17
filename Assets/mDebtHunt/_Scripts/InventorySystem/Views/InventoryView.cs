using System.Collections.Generic;
using UnityEngine;

public class InventoryView : MonoBehaviour
{
    [SerializeField] private InventorySlotView[] _slotViews;

    private InventoryService _inventoryService;

    public void Init(InventoryService inventoryService)
    {
        _inventoryService = inventoryService;
        _inventoryService.OnInventoryChanged += Refresh;
        Refresh();
    }

    private void OnDestroy()
    {
        if (_inventoryService != null)
            _inventoryService.OnInventoryChanged -= Refresh;
    }

    private void Refresh()
    {
        IReadOnlyList<InventorySlot> slots = _inventoryService.GetSlots();
        
        for (int i = 0; i < _slotViews.Length; i++)
        {
            if (i < slots.Count)
                _slotViews[i].Bind(slots[i]);
            else
                _slotViews[i].Bind(null);
        }
    }
}