using UnityEngine;
using System.Collections.Generic;
using System;

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
            _kitchenObjectSOLits.Add(kitchenObjectSO);

            OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs
            {
                kitchenObjectSO = kitchenObjectSO
            });

            return true;
        }
    }

    public List<KitchenObjectSO> GetKitchenObjectSOList() => _kitchenObjectSOLits;
}
