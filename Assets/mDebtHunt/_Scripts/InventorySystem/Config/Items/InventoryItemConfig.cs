using NaughtyAttributes;
using UnityEngine;

public abstract class InventoryItemConfig : ScriptableObject
{
    [field: SerializeField, BoxGroup("Main config"), HorizontalLine] public string ItemId { get; private set; }
    
    [field: SerializeField, BoxGroup("Main config"), HorizontalLine] public RarityType ItemRarity { get; private set; }
    
    [field: SerializeField, BoxGroup("Main config")] public string ItemName { get; private set; }
    
    [field: SerializeField, BoxGroup("Main config")] public Sprite ItemIcon { get; private set; }

    [field: SerializeField, BoxGroup("Main config")] public bool IsStackable { get; private set; }
    [field: SerializeField, BoxGroup("Main config"), MinValue(1)] public int Amount { get; private set; }
    [field: SerializeField, BoxGroup("Main config"), MinValue(1)] public int MaxCapacity { get; private set; }

    [field: SerializeField, BoxGroup("Main config")] public PickupbleInventoryItem ItemObject { get; private set; }
    [field: SerializeField, BoxGroup("Main config")] public Vector3 PositionHand { get; private set; }
    [field: SerializeField, BoxGroup("Main config")] public Vector3 RotationHand { get; private set; }
    
    private void OnValidate()
    {
        if (!IsStackable)
        {
            Amount = 1;
            MaxCapacity = 1;
            return;
        }
        
        MaxCapacity = Mathf.Max(1, MaxCapacity);
        Amount = Mathf.Clamp(Amount, 1, MaxCapacity);
    }
    
    public abstract void Use(GameplayProvider provider);
}