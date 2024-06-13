using UnityEngine;
using Unity.Netcode;

public class KitchenObject : NetworkBehaviour
{
    [SerializeField] private KitchenObjectSO _kitchenObjectSO;

    private IKitchenObjectParent _kitchenObjectParent;
    private FollowTransform _followTransform;

    protected virtual void Awake()
    {
        _followTransform = GetComponent<FollowTransform>();
    }

    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent)
    {
        SetKitchenObjectParentServerRpc(kitchenObjectParent.GetNetworkObject());
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetKitchenObjectParentServerRpc(NetworkObjectReference kitchenObjectParentNetworkObjectReference)
    {
        SetKitchenObjectParentClientRpc(kitchenObjectParentNetworkObjectReference);
    }

    [ClientRpc]
    private void SetKitchenObjectParentClientRpc(NetworkObjectReference kitchenObjectParentNetworkObjectReference)
    {
        kitchenObjectParentNetworkObjectReference.TryGet(out NetworkObject kitchenObjectParentNetworkObject);
        IKitchenObjectParent kitchenObjectParent = kitchenObjectParentNetworkObject.GetComponent<IKitchenObjectParent>();

        if (_kitchenObjectParent != null)
            _kitchenObjectParent.ClearKitchenObject();


        _kitchenObjectParent = kitchenObjectParent;

        if (kitchenObjectParent.HasKitchenObject())
        {
            //Error
        }

        kitchenObjectParent.SetKitchenObject(this);

        _followTransform.SetTargetTransform(kitchenObjectParent.GetKitchenObjectFollowTransform());
    }


    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    public void ClearKitchenObjectParent()
    {
        _kitchenObjectParent.ClearKitchenObject();
    }

    public IKitchenObjectParent GetKitchenObjectParent() => _kitchenObjectParent;
    public KitchenObjectSO GetKitchenObjectSO() => _kitchenObjectSO;

    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject)
    {
        if (this is PlateKitchenObject)
        {
            plateKitchenObject = this as PlateKitchenObject;

            return true;
        }
        else
        {
            plateKitchenObject = null;
            return false;
        }
    }

    public static void SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent parent)
    {
        KitchenGameMultiplayer.Instance.SpawnKitchenObject(kitchenObjectSO, parent);
    }

    public static void DestroyKitchenObject(KitchenObject kitchenObject)
    {
        KitchenGameMultiplayer.Instance.DestroyKitchenObject(kitchenObject);
    }
}
