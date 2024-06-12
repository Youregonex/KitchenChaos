using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{
    [SerializeField] private StoveCounter _stoveCounter;

    private float _warningSoundTimer;
    private float _warningSoundTimerMax = .2f;
    private bool _playWarningSound;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
        _stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChengedEEventArgs e)
    {
        float burnShowProgressAmount = .5f;

        _playWarningSound = _stoveCounter.IsFried() && e.progressNormalized >= burnShowProgressAmount;
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        bool playSound = e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried;

        if(playSound)
        {
            _audioSource.Play();
        }
        else
        {
            _audioSource.Pause();
        }
    }

    private void Update()
    {
        if(_playWarningSound)
        {
            _warningSoundTimer -= Time.deltaTime;

            if(_warningSoundTimer < 0)
            {
                _warningSoundTimer = _warningSoundTimerMax;

                SoundManager.Instance.PlayWarningSound(_stoveCounter.transform.position);
            }
        }
    }
}
