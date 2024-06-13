using UnityEngine;
using Unity.Netcode;

public class PlayerAnimator : NetworkBehaviour
{
    [SerializeField] private Player _player;

    private const string IS_WALKING = "IsWalking";
    private Animator _animator;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!IsOwner)
            return;

        _animator.SetBool(IS_WALKING, _player.IsWalking());
    }
}
