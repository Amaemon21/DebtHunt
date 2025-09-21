using R3;

public class InventorySlot
{
    private readonly InventorySlotData _data;
    
    private readonly ReactiveProperty<InventoryItemConfig> _item = new();
    public Observable<InventoryItemConfig> Item => _item;
    
    public InventoryItemConfig Config => _item.Value;
    
    public int Amount 
    { 
        get => _data.Amount;
        set => _data.Amount = value;
    }
    
    public int Index 
    { 
        get => _data.Index;
        private set => _data.Index = value;
    }
    
    public bool IsEmpty => _item.Value == null;
    
    public InventorySlot(InventorySlotData data, int index, ItemsDatabase itemsDatabase)
    {
        _data = data;
        Index = index;

        if (!string.IsNullOrEmpty(_data.ItemID))
        {
            _item.Value = itemsDatabase.GetItem(_data.ItemID);
        }
    }
    
    public void Set(InventoryItemConfig item, int amount)
    {
        _item.Value = item;
        _data.ItemID = item != null ? item.ItemId : null;
        Amount = amount;
    }

    public void Clear()
    {
        _item.Value = null;
        _data.ItemID = null;
        Amount = 0;
    }
    

}