using System.Collections.Generic;
using Unity.Netcode;
using System;

public class CharacterSelectReady : NetworkBehaviour
{
    public static CharacterSelectReady Instance { get; private set; }

    public event EventHandler OnReadyChanged;

    private Dictionary<ulong, bool> _playerReadyDictionary;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);

        Instance = this;

        _playerReadyDictionary = new Dictionary<ulong, bool>();
    }

    public void SetPlayerReady()
    {
        SetPlayerReadyServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
    {
        SetPlayerReadyClientRpc(serverRpcParams.Receive.SenderClientId);

        _playerReadyDictionary[serverRpcParams.Receive.SenderClientId] = true;

        bool allClientsReady = true;

        foreach (ulong clientID in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (!_playerReadyDictionary.ContainsKey(clientID) || !_playerReadyDictionary[clientID])
            {
                allClientsReady = false;
                break;
            }
        }

        if (allClientsReady)
        {
            KitchenGameLobby.Instance.DeleteLobby();

            Loader.LoadNetwork(Loader.Scene.GameScene);
        }
    }

    [ClientRpc]
    private void SetPlayerReadyClientRpc(ulong clientID)
    {   
        _playerReadyDictionary[clientID] = true;

        OnReadyChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool IsPlayerReady(ulong clientID) => _playerReadyDictionary.ContainsKey(clientID) && _playerReadyDictionary[clientID];
}
