using UnityEngine;
using Unity.Netcode;

public interface IKitchenObjectParent
{
    public void SetKitchenObject(KitchenObject kitchenObject);

    public KitchenObject GetKitchenObject();

    public void ClearKitchenObject();

    public bool HasKitchenObject();

    public Transform GetKitchenObjectFollowTransform();

    public NetworkObject GetNetworkObject();
}
