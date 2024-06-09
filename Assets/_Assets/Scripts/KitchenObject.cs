using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO _kitchenObjectSO;

    private ClearCounter _clearCounter;

    public void SetClearCounter(ClearCounter clearCounter) => _clearCounter = clearCounter;
    public ClearCounter GetClearCounter() => _clearCounter;
    public KitchenObjectSO GetKitchenObjectSO() => _kitchenObjectSO;

}
