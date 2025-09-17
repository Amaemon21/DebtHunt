using System.Collections.Generic;
using UnityEngine;

public class GameStatePlayerPrefsProvider : IGameStateProvider
{
    private readonly InventoryGridConfig _gridConfig;
        
    private const string KEY = "GAME STATE";
        
    public GameStateData GameState { get; private set; }
        
    public GameStatePlayerPrefsProvider(InventoryGridConfig gridConfig)
    {
        _gridConfig = gridConfig;
    }
        
    public void Save()
    {
        string json = JsonUtility.ToJson(GameState, true);
        Debug.Log("Saved JSON: " + json);
        PlayerPrefs.SetString(KEY, json);
        PlayerPrefs.Save();
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey(KEY))
        {
            var json = PlayerPrefs.GetString(KEY);
            Debug.Log("Loaded JSON: " + json);
            GameState = JsonUtility.FromJson<GameStateData>(json);
        }
        else
        {
            GameState = InitFromSettings();
            Save();
        }
    }

    private GameStateData InitFromSettings()
    {
        GameStateData gameState = new GameStateData
        {
            Inventory = CreateInventory(_gridConfig)
        };

        return gameState;
    }
        
    private InventoryGridData CreateInventory(InventoryGridConfig config)
    {
        var size = config.Сapacity;
        var createdInventorySlots = new List<InventorySlotData>();

        for (int i = 0; i < size; i++)
        {
            createdInventorySlots.Add(new InventorySlotData { Index = i, Amount = 0, ItemID = null });
        }

        InventoryGridData createdInventoryData = new InventoryGridData()
        {
            Capacity = size,
            Slots = createdInventorySlots
        };
        
        return createdInventoryData;
    }
}