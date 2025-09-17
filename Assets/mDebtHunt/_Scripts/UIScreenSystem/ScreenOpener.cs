using UnityEngine;
using Zenject;

public class ScreenOpener : MonoBehaviour
{
    [Inject] private readonly ScreenService _screenService;
    [Inject] private readonly InputSystem _inputSystem;

    private void OnEnable()
    {
        _inputSystem.InventoryChanged += OnInventoryScreenChanged;
    }
    
    private void OnDisable()
    {
        _inputSystem.InventoryChanged -= OnInventoryScreenChanged;
    }
    
    private void OnInventoryScreenChanged()
    {
        //_screenService.SwitchStateScreen(ScreenType.Inventory);
    }
}