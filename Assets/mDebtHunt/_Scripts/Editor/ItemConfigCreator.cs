#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;
using System;

public static class InventoryItemConfigCreator
{
    private const string BaseMenuPath = "Assets/Create/Inventory Item Config/";
    private const string TargetFolder = "Assets/mDebtHunt/InventoryConfigs/Configs";

    [MenuItem(BaseMenuPath + "Health Item Config")]
    private static void CreateHealthItemConfig()
    {
        CreateConfig(typeof(HealthItemConfig));
    }
    
    [MenuItem(BaseMenuPath + "Trap Item Config")]
    private static void CreateTrapItemConfig()
    {
        CreateConfig(typeof(TrapItemConfig));
    }

    [MenuItem(BaseMenuPath + "Other Item Config")]
    private static void CreateOtherItemConfig()
    {
        CreateConfig(typeof(OtherItemConfig));
    }

    private static void CreateConfig(Type configType)
    {
        UnityEngine.Object selected = Selection.activeObject;
        if (selected == null)
        {
            Debug.LogError("Ничего не выбрано!");
            return;
        }

        string assetPath = AssetDatabase.GetAssetPath(selected);
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);

        if (prefab == null)
        {
            EditorUtility.DisplayDialog("Ошибка", "Выбери префаб в Project!", "Ок");
            return;
        }

        // Создаём SO
        var config = ScriptableObject.CreateInstance(configType) as InventoryItemConfig;
        if (config == null)
        {
            Debug.LogError($"Не удалось создать {configType.Name}.");
            return;
        }

        // Имя = имя префаба + " Config"
        string prefabName = prefab.name;
        config.name = prefabName + " Config";

        // Проставляем ссылку на PickupbleInventoryItem
        var pickup = prefab.GetComponent<PickupbleInventoryItem>();
        if (pickup != null)
        {
            typeof(InventoryItemConfig).GetField("<ItemObject>k__BackingField", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)?.SetValue(config, pickup);
            typeof(InventoryItemConfig).GetField("<ItemId>k__BackingField", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)?.SetValue(config, prefabName);
        }

        // Убеждаемся, что папка для конфигов существует
        if (!AssetDatabase.IsValidFolder(TargetFolder))
        {
            Directory.CreateDirectory(TargetFolder);
            AssetDatabase.Refresh();
        }

        // Путь к новому конфигу
        string newAssetPath = AssetDatabase.GenerateUniqueAssetPath(
            Path.Combine(TargetFolder, prefabName + " Config.asset")
        );

        AssetDatabase.CreateAsset(config, newAssetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorGUIUtility.PingObject(config);
        Selection.activeObject = config;
    }

    [MenuItem(BaseMenuPath + "Health Item Config", true)]
    [MenuItem(BaseMenuPath + "Other Item Config", true)]
    private static bool ValidateCreateConfig()
    {
        if (Selection.activeObject == null) return false;
        string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        return !string.IsNullOrEmpty(path) && path.EndsWith(".prefab", StringComparison.OrdinalIgnoreCase);
    }
}
#endif
