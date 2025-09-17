using UnityEngine;

[CreateAssetMenu(fileName = "InventoryGridConfig", menuName = "Inventory System/Inventory Grid Config")]
public class InventoryGridConfig : ScriptableObject
{
    [field: SerializeField] public string OwnerId { get; private set; }
    [field: SerializeField] public int Сapacity { get; private set; }
}