using System.Collections.Generic;
using R3;

public class HotbarGrid
{
    private readonly List<InventorySlot> _slots;
    public int Capacity => _slots.Count;

    private ReactiveProperty<int> _activeIndex;
    public ReadOnlyReactiveProperty<int> ActiveIndex => _activeIndex;

    public HotbarGrid(int capacity)
    {
        _slots = new List<InventorySlot>(capacity);
        
        for (int i = 0; i < capacity; i++)
            _slots.Add(null); 
        
        _activeIndex = new ReactiveProperty<int>(0);
    }

    public int ActiveSlot()
    {
        return _activeIndex.Value;
    }
    
    public void AssignSlot(int hotbarIndex, InventorySlot inventorySlot)
    {
        if (hotbarIndex < 0 || hotbarIndex >= _slots.Count) 
            return;
        
        _slots[hotbarIndex] = inventorySlot;
    }
    
    public InventorySlot GetSlot(int index)
    {
        if (index < 0 || index >= _slots.Count)
            return null;
        
        return _slots[index];
    }

    public void SetActive(int index)
    {
        if (index < 0 || index >= _slots.Count) 
            return;
        
        _activeIndex.Value = index;
    }
}