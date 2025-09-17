using System;
using UnityEngine;
using Zenject;

public class InteractableSystem : MonoBehaviour
{
    [Inject] private readonly InventoryService _inventoryService;
    [Inject] private readonly InputSystem _inputSystem;

    [SerializeField] private LayerMask _hitScanMask;
    [SerializeField] private float _interactRange = 5f;

    private Camera _camera;
    private PickupbleInventoryItem _currentItem;
    
    public event Action OnItemPickupFailed;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void OnEnable() => _inputSystem.InteractChanged += PickupItem;

    private void OnDisable() => _inputSystem.InteractChanged -= PickupItem;

    private void Update()
    {
        CheckForInteractable();
    }

    private void CheckForInteractable()
    {
        Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f));

        if (Physics.Raycast(ray, out RaycastHit hit, _interactRange, _hitScanMask))
        {
            if (hit.collider.TryGetComponent(out PickupbleInventoryItem item))
            {
                HandleItem(item);

                return;
            }
        }

        Clear();
    }

    private void HandleItem(PickupbleInventoryItem item)
    {
        if (_currentItem != item)
            Clear();

        _currentItem = item;

        ShowInteract(item);
    }

    private void PickupItem()
    {
        if (_currentItem != null)
        {
            _inventoryService.AddItem(_currentItem.InventoryItemConfig);
            
            Destroy(_currentItem.gameObject);
            Clear();
            
          // if (result.ItemsAddedAmount > 0)
          // {

          // }
          // else
          // {
          //     OnItemPickupFailed?.Invoke();
          // }
        }
    }

    private void ShowInteract(PickupbleInventoryItem item)
    {
        if (item.Outline != null)
            item.Outline.enabled = true;
    }

    private void Clear()
    {
        if (_currentItem?.Outline != null)
            _currentItem.Outline.enabled = false;

        _currentItem = null;
    }
}