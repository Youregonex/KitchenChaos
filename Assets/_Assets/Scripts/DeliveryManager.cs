using UnityEngine;
using System.Collections.Generic;
using System;
using Unity.Netcode;

public class DeliveryManager : NetworkBehaviour
{
    public static DeliveryManager Instance { get; private set; }

    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailed;

    [SerializeField] private RecipeListSO _recipeListSO;

    private List<RecipeSO> _waitingRecipeSOList;

    private int _successfullRecipesDeliveredAmount;
    private float _spawnRecipeTimer = 4f;
    private float _spawnRecipeTimerMax = 4f;
    private int _waitingReecipesMax = 4;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);

        Instance = this;

        _waitingRecipeSOList = new();
    }

    private void Update()
    {
        if (!IsServer)
            return;

        _spawnRecipeTimer -= Time.deltaTime;

        if(_spawnRecipeTimer <= 0f)
        {
            _spawnRecipeTimer = _spawnRecipeTimerMax;

            if(KitchenGameManager.Instance.IsGamePlaying() && _waitingRecipeSOList.Count < _waitingReecipesMax)
            {
                int waitingRecipeSOIndex = UnityEngine.Random.Range(0, _recipeListSO.recipeSOList.Count);

                SpawnNewWaitingRecipeClientRpc(waitingRecipeSOIndex);
            }
        }
    }

    [ClientRpc]
    private void SpawnNewWaitingRecipeClientRpc(int waitingRecipeSOIndex)
    {
        RecipeSO waitingRecipeSO = _recipeListSO.recipeSOList[waitingRecipeSOIndex];

        _waitingRecipeSOList.Add(waitingRecipeSO);

        OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for (int i = 0; i < _waitingRecipeSOList.Count; i++)
        {
            RecipeSO waitingRecipeSO = _waitingRecipeSOList[i];

            if(waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
            {
                bool plateContentMatchesRecipe = true;

                foreach (KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList)
                {
                    bool ingridientFound = false;

                    foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
                    {
                        if(plateKitchenObjectSO == recipeKitchenObjectSO)
                        {
                            ingridientFound = true;
                            break;
                        }

                    }

                    if(!ingridientFound)
                    {
                        plateContentMatchesRecipe = false;
                    }
                }

                if(plateContentMatchesRecipe)
                {
                    DeliverCorrectRecipeServerRpc(i);

                    return;
                }
            }
        }

        DeliverIncorrectRecipeServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void DeliverCorrectRecipeServerRpc(int waitingRecipeSOListIndex)
    {
        DeliverCorrectRecipeClientRpc(waitingRecipeSOListIndex);
    }

    [ClientRpc]
    private void DeliverCorrectRecipeClientRpc(int waitingRecipeSOListIndex)
    {
        _waitingRecipeSOList.RemoveAt(waitingRecipeSOListIndex);

        _successfullRecipesDeliveredAmount++;

        OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
        OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
    }

    [ServerRpc(RequireOwnership = false)]
    private void DeliverIncorrectRecipeServerRpc()
    {
        DeliverIncorrectRecipeClientRpc();
    }

    [ClientRpc]
    private void DeliverIncorrectRecipeClientRpc()
    {
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
    }

    public List<RecipeSO> GetWaitingRecipeSOList() => _waitingRecipeSOList;
    public int GetSuccessfullRecipesAmount() => _successfullRecipesDeliveredAmount;
}
