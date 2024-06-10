using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    [SerializeField] protected Transform _counterTopPoint;

    private KitchenObject _kitchenObject;

    public virtual void Interact(Player player) { Debug.LogError("Interact on BaseCounter"); }

    public virtual void InteractAlternate(Player player) { Debug.LogError("InteractAlternate on BaseCounter"); }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        _kitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject() => _kitchenObject;

    public void ClearKitchenObject()
    {
        _kitchenObject = null;
    }

    public bool HasKitchenObject() => _kitchenObject != null ? true : false;

    public Transform GetKitchenObjectFollowTransform() => _counterTopPoint;
}
