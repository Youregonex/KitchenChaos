using UnityEngine;
using System;

public class KitchenGameManager : MonoBehaviour
{
    public static KitchenGameManager Instance { get; private set; }

    private enum State
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver
    }

    private State _state;

    public event EventHandler OnStateChanged;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;

    private float _waitingToStartTimer = 1f;
    private float _countdownToStartTimer = 3f;
    private float _gamePlayingTimer;
    private float _gamePlayingTimerMax = 10f;
    private bool _isGamePaused = false;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);

        Instance = this;

        _state = State.WaitingToStart;
    }

    private void Start()
    {
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
    }

    private void GameInput_OnPauseAction(object sender, EventArgs e)
    {
        TogglePauseGame();
    }

    private void Update()
    {
        switch(_state)
        {
            case State.WaitingToStart:

                _waitingToStartTimer -= Time.deltaTime;

                if(_waitingToStartTimer < 0)
                {
                    _state = State.CountdownToStart;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }

                break;
            case State.CountdownToStart:

                _countdownToStartTimer -= Time.deltaTime;

                if (_countdownToStartTimer < 0)
                {
                    _gamePlayingTimer = _gamePlayingTimerMax;

                    _state = State.GamePlaying;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }

                break;
            case State.GamePlaying:

                _gamePlayingTimer -= Time.deltaTime;

                if (_gamePlayingTimer < 0)
                {
                    _state = State.GameOver;
                    OnStateChanged?.Invoke(this, EventArgs.Empty);
                }

                break;
            case State.GameOver:
                break;
        }
    }

    public void TogglePauseGame()
    {
        _isGamePaused = !_isGamePaused;

        if(_isGamePaused)
        {
            Time.timeScale = 0f;
            OnGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Time.timeScale = 1f;
            OnGameUnpaused?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool IsGamePlaying() => _state == State.GamePlaying;
    public bool IsCountdownToStartActive() => _state == State.CountdownToStart;
    public float GetCountdownToStartTimer() => _countdownToStartTimer;
    public bool IsGameOveer() => _state == State.GameOver;
    public float GetGamePlayingTimerNormalized() => 1 - (_gamePlayingTimer / _gamePlayingTimerMax);
}
