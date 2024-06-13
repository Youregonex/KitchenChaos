using UnityEngine;
using System;
using Unity.Netcode;

public class PlatesCounter : BaseCounter
{

    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;

    [SerializeField] private KitchenObjectSO _plateKitchenObjectSO;

    private float _spawnPlateTimer;
    private float _spawnPlateTimerMax = 4f;
    private int _platesSpawnedAmount;
    private int _platesSpawnedAmountMax = 4;

    private void Update()
    {
        if (!IsServer)
            return;

        _spawnPlateTimer += Time.deltaTime;

        if(_spawnPlateTimer >= _spawnPlateTimerMax)
        {
            _spawnPlateTimer = 0f;

            if(KitchenGameManager.Instance.IsGamePlaying() && _platesSpawnedAmount < _platesSpawnedAmountMax)
            {
                SpawnPlateServerRpc();
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnPlateServerRpc()
    {
        SpawnPlateClientRpc();
    }

    [ClientRpc]
    private void SpawnPlateClientRpc()
    {
        _platesSpawnedAmount++;

        OnPlateSpawned?.Invoke(this, EventArgs.Empty);
    }

    public override void Interact(Player player)
    {
        if(!player.HasKitchenObject())
        {
            if(_platesSpawnedAmount > 0)
            {
                KitchenObject.SpawnKitchenObject(_plateKitchenObjectSO, player);
                InteractLogicServerRpc();
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void InteractLogicServerRpc()
    {
        InteractLogicClientRpc();
    }

    [ClientRpc]
    private void InteractLogicClientRpc()
    {
        _platesSpawnedAmount--;
        OnPlateRemoved?.Invoke(this, EventArgs.Empty);
    }
}
