using UnityEngine;
using Unity.Netcode;

public class KitchenGameMultiplayer : NetworkBehaviour
{
    public static KitchenGameMultiplayer Instance { get; private set; }

    [SerializeField] private KitchenObjectListSO _kitchenObjectListSO;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);

        Instance = this;
    }

    public void SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent)
    {
        SpawnKitchenObjectServerRpc(GetKitchenSOIndex(kitchenObjectSO), kitchenObjectParent.GetNetworkObject());
    }

    
    [ServerRpc(RequireOwnership = false)]
    private void SpawnKitchenObjectServerRpc(int kitchenObjectSOIndex, NetworkObjectReference kitchenObjectParentNetworkObjectReference)
    {
        KitchenObjectSO kitchenObjectSO = GetKitchenObjectSOFromIndex(kitchenObjectSOIndex);

        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.ObjectPrefab);

        NetworkObject kitchenObjectNetworkObject = kitchenObjectTransform.GetComponent<NetworkObject>();

        kitchenObjectNetworkObject.Spawn(true);

        KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();

        kitchenObjectParentNetworkObjectReference.TryGet(out NetworkObject kitchenObjectParentNetworkObject);
        IKitchenObjectParent kitchenObjectParent = kitchenObjectParentNetworkObject.GetComponent<IKitchenObjectParent>();

        kitchenObject.SetKitchenObjectParent(kitchenObjectParent);
    }

    private int GetKitchenSOIndex(KitchenObjectSO kitchnObjectSO)
    {
        return _kitchenObjectListSO.kitchenObjectSOList.IndexOf(kitchnObjectSO);
    }

    private KitchenObjectSO GetKitchenObjectSOFromIndex(int kitchenObjectSOIndex)
    {
        return _kitchenObjectListSO.kitchenObjectSOList[kitchenObjectSOIndex];
    }

    public void DestroyKitchenObject(KitchenObject kitchenObject)
    {
        DestroyKitchenObjectServerRpc(kitchenObject.NetworkObject);
    }

    [ServerRpc(RequireOwnership = false)]
    private void DestroyKitchenObjectServerRpc(NetworkObjectReference kitchenObjectNetworkReference)
    {
        kitchenObjectNetworkReference.TryGet(out NetworkObject kitchenObjectNetworkObject);

        KitchenObject kitchenObject = kitchenObjectNetworkObject.GetComponent<KitchenObject>();


        ClearKitchenObjectParentClientRpc(kitchenObjectNetworkReference);

        kitchenObject.DestroySelf();
    }

    [ClientRpc]
    private void ClearKitchenObjectParentClientRpc(NetworkObjectReference kitchenObjectNetworkReference)
    {
        kitchenObjectNetworkReference.TryGet(out NetworkObject kitchenObjectNetworkObject);

        KitchenObject kitchenObject = kitchenObjectNetworkObject.GetComponent<KitchenObject>();

        kitchenObject.ClearKitchenObjectParent();
    }
}
