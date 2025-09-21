using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HotbarSlotView : MonoBehaviour
{
    [SerializeField] private Image _slotImage;
    [SerializeField] private Image _iconImage;
    
    [SerializeField] private TextMeshProUGUI _amountText;
    
    [SerializeField] private DisplaySlotIconProperty _displaySlotIconProperty;
    
    private InventorySlot _slot;

    public void Bind(InventorySlot slot, bool active)
    {
        _slot = slot;
        Refresh(active);
    }

    private void Refresh(bool active)
    {
        if (_slot == null || _slot.IsEmpty)
        {
            _iconImage.enabled = false;
            _amountText.text = "";
        }
        else
        {
            _iconImage.enabled = true;
            _iconImage.sprite = _slot.Config.ItemIcon;

            _amountText.text = _slot.Config.IsStackable && _slot.Amount > 1 ? _slot.Amount.ToString() : "";
        }
        
        DisplaySlotIconByItemType(active);
    }
    
    private void DisplaySlotIconByItemType(bool highlighted = false)
    {
        if (_slot == null)
            return;

        if (_slot.IsEmpty)
        {
            _slotImage.sprite = highlighted ? _displaySlotIconProperty.HighlightedCellSprite : _displaySlotIconProperty.EmptyCellSprite;
            return;
        }
        
        _slotImage.sprite = highlighted ? _displaySlotIconProperty.HighlightedCellSprite : _displaySlotIconProperty.NonEmptyCellSprite;
    }
}