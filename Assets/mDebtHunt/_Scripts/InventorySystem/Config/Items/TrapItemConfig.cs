using UnityEngine;

[CreateAssetMenu(fileName = "TrapItemConfig", menuName = "Inventory System/Trap Item Config")]
public class TrapItemConfig : InventoryItemConfig
{
    [SerializeField] private LayerMask _groundMask;      

    private Camera _camera;
    private InventoryService _inventoryService;
    private HotbarController _hotbarController;

    public override void Use(GameplayProvider provider)
    {
        _camera = provider.Get<CameraSystem>().Camera;
        _inventoryService = provider.Get<InventoryService>();
        _hotbarController = provider.Get<HotbarController>();
        
        Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);
        
        if (Physics.Raycast(ray, out RaycastHit hit, 10f, _groundMask))
        {
            Instantiate(ItemObject, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
            
            _inventoryService.RemoveItem(this, _hotbarController.CurrentIndex);
        }
    }
}