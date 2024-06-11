using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    private Player _player;
    private float _footStepTimer;
    private float _footStepTimerMax = .1f;


    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    private void Update()
    {
        _footStepTimer -= Time.deltaTime;

        if(_footStepTimer < 0)
        {
            _footStepTimer = _footStepTimerMax;

            if(_player.IsWalking())
            {
                float footstepVolume = 1f;
                SoundManager.Instance.PlayFootstepsSound(_player.transform.position, footstepVolume);
            }
        }
    }
}
