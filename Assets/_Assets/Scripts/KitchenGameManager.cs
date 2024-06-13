using UnityEngine;
using System;
using System.Collections.Generic;
using Unity.Netcode;

public class KitchenGameManager : NetworkBehaviour
{
    public static KitchenGameManager Instance { get; private set; }

    private enum State
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver
    }

    private NetworkVariable<State> _state = new NetworkVariable<State>(State.WaitingToStart);
    private NetworkVariable<float> _countdownToStartTimer = new NetworkVariable<float>(3f);
    private NetworkVariable<float> _gamePlayingTimer = new NetworkVariable<float>(0f);
    private NetworkVariable<bool> _isGamePaused = new NetworkVariable<bool>(false);

    public event EventHandler OnStateChanged;
    public event EventHandler OnLocalGamePaused;
    public event EventHandler OnLocalGameUnpaused;
    public event EventHandler OnLocalPlayerReadyChanged;
    public event EventHandler OnMultiplayerGamePaused;
    public event EventHandler OnMultiplayerGameUnpaused;

    private float _gamePlayingTimerMax = 90f;
    private bool _isLocalPlayerReady;
    private bool _isLocalGamePaused = false;
    private bool _autoTestGamePausedState;

    private Dictionary<ulong, bool> _playerReadyDictionary;
    private Dictionary<ulong, bool> _playerPausedDictionary;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);

        Instance = this;

        _playerReadyDictionary = new Dictionary<ulong, bool>();
        _playerPausedDictionary = new Dictionary<ulong, bool>();
    }

    private void Start()
    {
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
    }

    public override void OnNetworkSpawn()
    {
        _state.OnValueChanged += State_OnValueChanged;
        _isGamePaused.OnValueChanged += IsGamePaused_OnValueChenged;

        if(IsServer)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
        }
    }

    private void NetworkManager_OnClientDisconnectCallback(ulong clientID)
    {
        _autoTestGamePausedState = true;
    }

    private void IsGamePaused_OnValueChenged(bool previuoseValue, bool newValue)
    {
        if (_isGamePaused.Value)
        {
            Time.timeScale = 0f;

            OnMultiplayerGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Time.timeScale = 1f;

            OnMultiplayerGameUnpaused?.Invoke(this, EventArgs.Empty);
        }
    }

    private void State_OnValueChanged(State previuoseValue, State newValue)
    {
        OnStateChanged?.Invoke(this, EventArgs.Empty);
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if(_state.Value == State.WaitingToStart)
        {
            _isLocalPlayerReady = true;

            OnLocalPlayerReadyChanged?.Invoke(this, EventArgs.Empty);

            SetPlayerReadyServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
    {
        _playerReadyDictionary[serverRpcParams.Receive.SenderClientId] = true;

        bool allClientsReady = true;

        foreach(ulong clientID in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if(!_playerReadyDictionary.ContainsKey(clientID) || !_playerReadyDictionary[clientID])
            {
                allClientsReady = false;
                break;
            }
        }

        if(allClientsReady)
        {
            _state.Value = State.CountdownToStart;
        }
    }

    private void GameInput_OnPauseAction(object sender, EventArgs e)
    {
        TogglePauseGame();
    }

    private void Update()
    {
        if (!IsServer)
            return;

        switch(_state.Value)
        {
            case State.WaitingToStart:
                break;

            case State.CountdownToStart:

                _countdownToStartTimer.Value -= Time.deltaTime;

                if (_countdownToStartTimer.Value < 0)
                {
                    _gamePlayingTimer.Value = _gamePlayingTimerMax;

                    _state.Value = State.GamePlaying;
                }

                break;

            case State.GamePlaying:

                _gamePlayingTimer.Value -= Time.deltaTime;

                if (_gamePlayingTimer.Value < 0)
                {
                    _state.Value = State.GameOver;
                }

                break;

            case State.GameOver:
                break;
        }
    }

    private void LateUpdate()
    {
        if(_autoTestGamePausedState)
        {
            _autoTestGamePausedState = false;
            TestGamePausedState();
        }
    }

    public void TogglePauseGame()
    {
        _isLocalGamePaused = !_isLocalGamePaused;

        if(_isLocalGamePaused)
        {
            PauseGameServerRpc();

            OnLocalGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            UnpauseGameServerRpc();

            OnLocalGameUnpaused?.Invoke(this, EventArgs.Empty);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void PauseGameServerRpc(ServerRpcParams serverRpcParams = default)
    {
        _playerPausedDictionary[serverRpcParams.Receive.SenderClientId] = true;

        TestGamePausedState();
    }

    [ServerRpc(RequireOwnership = false)]
    private void UnpauseGameServerRpc(ServerRpcParams serverRpcParams = default)
    {
        _playerPausedDictionary[serverRpcParams.Receive.SenderClientId] = false;

        TestGamePausedState();
    }

    private void TestGamePausedState()
    {
        foreach(ulong clientID in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if(_playerPausedDictionary.ContainsKey(clientID) && _playerPausedDictionary[clientID])
            {
                _isGamePaused.Value = true;

                return;
            }
        }

        _isGamePaused.Value = false;
    }

    public bool IsWaitingToStart() => _state.Value == State.WaitingToStart;
    public bool IslocalPlayerReady() => _isLocalPlayerReady;
    public bool IsGamePlaying() => _state.Value == State.GamePlaying;
    public bool IsCountdownToStartActive() => _state.Value == State.CountdownToStart;
    public float GetCountdownToStartTimer() => _countdownToStartTimer.Value;
    public bool IsGameOveer() => _state.Value == State.GameOver;
    public float GetGamePlayingTimerNormalized() => 1 - (_gamePlayingTimer.Value / _gamePlayingTimerMax);
}
