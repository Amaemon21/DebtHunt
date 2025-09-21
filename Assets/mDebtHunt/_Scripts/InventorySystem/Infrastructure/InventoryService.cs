using System;
using System.Collections.Generic;

public class InventoryService : IComponent
{
    private readonly IGameStateSaver _gameStateSaver;
    
    private InventoryGrid _inventoryGrid;
    
    public event Action OnInventoryChanged;

    public InventoryService(IGameStateSaver gameStateSaver)
    {
        _gameStateSaver = gameStateSaver;
    }

    public void RegisterInventory(InventoryGridData data, ItemsDatabase itemsDatabase)
    {
        _inventoryGrid = new InventoryGrid(data, itemsDatabase);
    }
    
    public AddItemsToInventoryGridResult AddItem(InventoryItemConfig config)
    {
        var result = _inventoryGrid.AddItems(config, config.Amount);
        _gameStateSaver.Save();
        OnInventoryChanged?.Invoke();
        return result;
    }

    public void AddItem(InventoryItemConfig config, int index)
    {
        _inventoryGrid.AddItems(config, index);
        _gameStateSaver.Save();
        OnInventoryChanged?.Invoke();
    }

    public void RemoveItem(InventoryItemConfig config)
    {
        _inventoryGrid.RemoveItems(config, config.Amount);
        _gameStateSaver.Save();
        OnInventoryChanged?.Invoke();
    }

    public void RemoveItem(InventoryItemConfig config, int index)
    {
        _inventoryGrid.RemoveItems(config, config.Amount, index);
        _gameStateSaver.Save();
        OnInventoryChanged?.Invoke();
    }

    public void SwapItems(int fromIndex, int toIndex)
    {
        _inventoryGrid.SwapItems(fromIndex, toIndex);
        _gameStateSaver.Save();
        OnInventoryChanged?.Invoke();
    }

    public bool HasItem(InventoryItemConfig config)
    {
        int count = 0;
        
        foreach (var slot in _inventoryGrid.Slots)
        {
            if (slot.Config == config)
                count += slot.Amount;

            if (count >= config.Amount)
                return true;
        }
        
        return false;
    }

    public int GetItemCount(InventoryItemConfig config)
    {
        int count = 0;
        foreach (var slot in _inventoryGrid.Slots)
        {
            if (slot.Config == config)
                count += slot.Amount;
        }
        return count;
    }

    public int GetСapacity() => _inventoryGrid.Сapacity;
    public IReadOnlyList<InventorySlot> GetSlots() => _inventoryGrid.Slots;
}