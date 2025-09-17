using UnityEngine;

public class HotbarView : MonoBehaviour
{
    [SerializeField] private HotbarSlotView[] _slotViews;

    private HotbarGrid _hotbarGrid;

    public void Init(HotbarGrid hotbarGrid)
    {
        _hotbarGrid = hotbarGrid;
        Refresh();
    }

    public void Refresh()
    {
        for (int i = 0; i < _slotViews.Length; i++)
        {
            var slot = _hotbarGrid.GetSlot(i);
            bool isActive = i == _hotbarGrid.ActiveSlot();
            _slotViews[i].Bind(slot, isActive);
        }
    }

    public void SetActive(int index)
    {
        _hotbarGrid.SetActive(index);
        Refresh();
    }
}