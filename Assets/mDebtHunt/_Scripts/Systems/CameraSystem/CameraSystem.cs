using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraSystem : MonoBehaviour, IComponent
{
    [field: SerializeField] public Camera Camera { get; private set; }
    
    private void Awake()
    {
        if (Camera == null)
        {
            Camera = GetComponent<Camera>();
        }
    }
}