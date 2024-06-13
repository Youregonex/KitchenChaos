using UnityEngine;
using System.Collections.Generic;
using System;
using Unity.Netcode;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectSO kitchenObjectSO;
    }

    [SerializeField] private List<KitchenObjectSO> _validKitchenObjectSOList; 

    private List<KitchenObjectSO> _kitchenObjectSOLits;


    protected override void Awake()
    {
        base.Awake();

        _kitchenObjectSOLits = new();
    }

    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
    {
        if (!_validKitchenObjectSOList.Contains(kitchenObjectSO))
            return false;

        if(_kitchenObjectSOLits.Contains(kitchenObjectSO))
        {
            return false;
        }
        else
        {
            int kitchenObjectSOIndex = KitchenGameMultiplayer.Instance.GetKitchenSOIndex(kitchenObjectSO);

            AddIngredientServerRpc(kitchenObjectSOIndex);

            return true;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void AddIngredientServerRpc(int kitchenObjectSoIndex)
    {
        AddIngredientClientRpc(kitchenObjectSoIndex);
    }

    [ClientRpc]
    private void AddIngredientClientRpc(int kitchenObjectSoIndex)
    {
        KitchenObjectSO kitchenObjectSO = KitchenGameMultiplayer.Instance.GetKitchenObjectSOFromIndex(kitchenObjectSoIndex);

        _kitchenObjectSOLits.Add(kitchenObjectSO);

        OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs
        {
            kitchenObjectSO = kitchenObjectSO
        });
    }

    public List<KitchenObjectSO> GetKitchenObjectSOList() => _kitchenObjectSOLits;
}
