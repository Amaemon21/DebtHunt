using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Inventory
{
    public class InventorySlotDraggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Inject] private readonly InventoryService _inventoryService;
        
        [SerializeField] private InventorySlotView _inventorySlotView;
        [SerializeField] private DraggableSlot _draggableSlot;

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!_inventorySlotView.Slot.IsEmpty)
            {
                _draggableSlot.gameObject.SetActive(true);

                _draggableSlot.Icon.sprite = _inventorySlotView.Slot.Config.ItemIcon;
                
                _draggableSlot.SlotCoordA = _inventorySlotView.Slot.Index;
            }
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            _draggableSlot.transform.position = Input.mousePosition;
        }
        
        public void OnEndDrag(PointerEventData eventData)
        {
            if (eventData.pointerCurrentRaycast.gameObject != null)
            {
                if (eventData.pointerCurrentRaycast.gameObject.TryGetComponent<InventorySlotView>(out var inventorySlotView))
                {
                    _draggableSlot.SlotCoordB = inventorySlotView.Slot.Index;
                    
                    _inventoryService.SwapItems(_draggableSlot.SlotCoordA, _draggableSlot.SlotCoordB);
                }
            }
            
            _draggableSlot.gameObject.SetActive(false);
        }
    }
}