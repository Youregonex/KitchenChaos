
public class DeliveryCounter : BaseCounter
{
    public static DeliveryCounter Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);

        Instance = this;
    }

    public override void Interact(Player player)
    {
        if(player.HasKitchenObject())
        {
            if(player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
            {

                DeliveryManager.Instance.DeliverRecipe(plateKitchenObject);

                KitchenObject.DestroyKitchenObject(player.GetKitchenObject());
            }
        }
    }
}
