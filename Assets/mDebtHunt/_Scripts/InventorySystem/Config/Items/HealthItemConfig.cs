using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItemConfig", menuName = "Inventory System/Health Item Config")]
public class HealthItemConfig : InventoryItemConfig
{
    [SerializeField] private int _healAmount = 50;

    public override void Use(GameplayProvider provider)
    {
        Debug.Log($"Лечение игрока на {_healAmount} HP");
    }
}