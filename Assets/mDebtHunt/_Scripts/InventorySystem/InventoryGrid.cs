using System;
using System.Collections.Generic;
using R3;

public class InventoryGrid
{
    private readonly ItemsDatabase _itemsDatabase;
    private readonly List<InventorySlot> _slots;
    private readonly InventoryGridConfig _config;
    
    public IReadOnlyList<InventorySlot> Slots => _slots;
    
    public InventoryGrid(InventoryGridData gridData, ItemsDatabase itemsDatabase)
    {
        _itemsDatabase = itemsDatabase;
        
        _slots = new List<InventorySlot>(gridData.Capacity);

        for (int i = 0; i < gridData.Capacity; i++)
        {
            _slots.Add(new InventorySlot(gridData.Slots[i], i, itemsDatabase));
        }
    }
    
    public AddItemsToInventoryGridResult AddItems(InventoryItemConfig config, int amount)
    {
        int added = 0;
        int remaining = amount;
        
        if (config.IsStackable)
        {
            foreach (var slot in _slots)
            {
                if (slot.GetItem() == null) continue;

                if (slot.GetItem().ItemId == config.ItemId && slot.Amount < config.MaxCapacity)
                {
                    int canAdd = config.MaxCapacity - slot.Amount;
                    int toAdd = Math.Min(canAdd, remaining);

                    slot.Amount += toAdd;
                    added += toAdd;
                    remaining -= toAdd;

                    if (remaining <= 0)
                        return new AddItemsToInventoryGridResult(slot.Amount, added);
                }
            }
        }
        
        foreach (var slot in _slots)
        {
            if (slot.IsEmpty)
            {
                int space = config.IsStackable ? config.MaxCapacity : 1;
                int toAdd = Math.Min(space, remaining);

                slot.Set(config, toAdd);
                added += toAdd;
                remaining -= toAdd;

                if (remaining <= 0)
                    return new AddItemsToInventoryGridResult(slot.Amount, added);
            }
        }

        return new AddItemsToInventoryGridResult(0, added);
    }

    
    public void AddItems(InventoryItemConfig config, int amount, int indexSlot)
    {
        if (indexSlot < 0 || indexSlot >= _slots.Count) return;

        var slot = _slots[indexSlot];

        if (slot.IsEmpty)
        {
            int toAdd = config.IsStackable ? Math.Min(config.MaxCapacity, amount) : 1;
            slot.Set(config, toAdd);
        }
        else if (slot.GetItem() == config && config.IsStackable && slot.Amount < config.MaxCapacity)
        {
            int canAdd = config.MaxCapacity - slot.Amount;
            slot.Amount += Math.Min(canAdd, amount);
        }
    }
    
    public void RemoveItems(InventoryItemConfig config, int amount)
    {
        foreach (var slot in _slots)
        {
            if (slot.GetItem().ItemId == config.ItemId)
            {
                int toRemove = Math.Min(amount, slot.Amount);
                slot.Amount -= toRemove;
                amount -= toRemove;

                if (slot.Amount <= 0) 
                    slot.Clear();
                
                if (amount <= 0) 
                    return;
            }
        }
    }

    public void RemoveItems(InventoryItemConfig config, int amount, int index)
    {
        if (index < 0 || index >= _slots.Count) 
            return;

        var slot = _slots[index];
        
        if (slot.GetItem().ItemId == config.ItemId)
        {
            int toRemove = Math.Min(amount, slot.Amount);
            slot.Amount -= toRemove;
            if (slot.Amount <= 0) slot.Clear();
        }
    }

    public void SwapItems(int fromIndex, int toIndex)
    {
        if (fromIndex < 0 || fromIndex >= _slots.Count) return;
        if (toIndex < 0 || toIndex >= _slots.Count) return;
        if (fromIndex == toIndex) return;

        var fromSlot = _slots[fromIndex];
        var toSlot = _slots[toIndex];

        if (fromSlot.Item != null && toSlot.Item == fromSlot.Item && fromSlot.GetItem().IsStackable)
        {
            int canAdd = fromSlot.GetItem().MaxCapacity - toSlot.Amount;
            
            if (canAdd > 0)
            {
                int toMove = Math.Min(canAdd, fromSlot.Amount);
                toSlot.Amount += toMove;
                fromSlot.Amount -= toMove;

                if (fromSlot.Amount <= 0) 
                    fromSlot.Clear();
                
                return;
            }
        }

        InventoryItemConfig tempItem = fromSlot.GetItem();
        int tempAmount = fromSlot.Amount;

        fromSlot.Set(toSlot.GetItem(), toSlot.Amount);
        toSlot.Set(tempItem, tempAmount);
    }
}