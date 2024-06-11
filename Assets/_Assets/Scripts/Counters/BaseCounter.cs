using UnityEngine;
using System;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    public static event EventHandler OnAnyObjectPlacedHere;

    [SerializeField] protected Transform _counterTopPoint;

    private KitchenObject _kitchenObject;

    public static void ResetStaticData()
    {
        OnAnyObjectPlacedHere = null;
    }

    public virtual void Interact(Player player)
    {
        Debug.LogError("Interact on BaseCounter");
    }

    public virtual void InteractAlternate(Player player)
    {
        //Debug.LogError("InteractAlternate on BaseCounter");
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        _kitchenObject = kitchenObject;

        if (kitchenObject != null)
            OnAnyObjectPlacedHere?.Invoke(this, EventArgs.Empty);
    }

    public KitchenObject GetKitchenObject() => _kitchenObject;

    public void ClearKitchenObject()
    {
        _kitchenObject = null;
    }

    public bool HasKitchenObject() => _kitchenObject != null ? true : false;

    public Transform GetKitchenObjectFollowTransform() => _counterTopPoint;
}
