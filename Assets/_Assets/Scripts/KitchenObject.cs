using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO _kitchenObjectSO;

    private IKitchenObjectParent _kitchenObjectParent;

    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent)
    {
        if (_kitchenObjectParent != null)
            _kitchenObjectParent.ClearKitchenObject();


        _kitchenObjectParent = kitchenObjectParent;

        if(kitchenObjectParent.HasKitchenObject())
        {
            //Error
        }

        kitchenObjectParent.SetKitchenObject(this);

        transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;
    }
    public void DestroySelf()
    {
        _kitchenObjectParent.ClearKitchenObject();

        Destroy(gameObject);
    }

    public IKitchenObjectParent GetKitchenObjectParent() => _kitchenObjectParent;
    public KitchenObjectSO GetKitchenObjectSO() => _kitchenObjectSO;

    public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent parent)
    {
        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.ObjectPrefab);
        KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
        kitchenObject.SetKitchenObjectParent(parent);

        return kitchenObject;
    }
}
