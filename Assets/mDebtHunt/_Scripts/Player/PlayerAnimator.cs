using UnityEngine;
using Zenject;

public class PlayerAnimator : MonoBehaviour
{
    private readonly int IsWalking = Animator.StringToHash("IsWalking");
    
    [Inject] private readonly InputSystem _inputSystem;
    
    [SerializeField] private Animator _animator;
    
    private bool _isWalking;

    private void Update()
    {
        _isWalking = _inputSystem.MoveDirection.sqrMagnitude > 0.01f;

        _animator.SetBool(IsWalking, _isWalking);
    }
}