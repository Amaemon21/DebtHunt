public readonly struct AddItemsToInventoryGridResult
{
    public readonly int ItemsToAddAmount;
    public readonly int ItemsAddedAmount;
        
    public int ItemsNotAddedAmount => ItemsAddedAmount - ItemsNotAddedAmount;

    public AddItemsToInventoryGridResult(int itemsToAddAmount, int itemsAddedAmount)
    {
        ItemsToAddAmount = itemsToAddAmount;
        ItemsAddedAmount = itemsAddedAmount;
    }
}