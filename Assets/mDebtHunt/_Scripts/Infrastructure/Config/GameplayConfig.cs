using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "GameplayConfig", menuName = "Config/GameplayConfig")]
public class GameplayConfig : ScriptableObject
{
    [field: SerializeField, BoxGroup("Inventory"), HorizontalLine] public ItemsDatabase ItemsDatabase { get; private set; }
    [field: SerializeField, BoxGroup("Inventory")] public InventoryGridConfig InventoryGridConfig { get; private set; }
}