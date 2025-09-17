using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlotView : MonoBehaviour
{
    [SerializeField] private Image _iconImage;
    [SerializeField] private TextMeshProUGUI _amountText;
    
    private InventorySlot _slot;
    
    public InventorySlot Slot => _slot;

    public void Bind(InventorySlot slot)
    {
        _slot = slot;
        Refresh();
    }

    public void Refresh()
    {
        if (_slot == null || _slot.IsEmpty)
        {
            _iconImage.enabled = false;
            _amountText.text = "";
            return;
        }

        _iconImage.enabled = true;
        _iconImage.sprite = _slot.GetItem().ItemIcon;

        if (_slot.GetItem().IsStackable && _slot.Amount > 1)
            _amountText.text = _slot.Amount.ToString();
        else
            _amountText.text = "";
    }
}