using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HotbarSlotView : MonoBehaviour
{
    [SerializeField] private Image _iconImage;
    [SerializeField] private TextMeshProUGUI _amountText;
    [SerializeField] private Image _highlight;

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
            _iconImage.sprite = _slot.GetItem().ItemIcon;

            _amountText.text = _slot.GetItem().IsStackable && _slot.Amount > 1 ? _slot.Amount.ToString() : "";
        }

        _highlight.enabled = active;
    }
}