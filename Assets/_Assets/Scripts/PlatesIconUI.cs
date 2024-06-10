using UnityEngine;

public class PlatesIconUI : MonoBehaviour
{
    [SerializeField] private PlateKitchenObject _plateKitchenObject;

    private void Start()
    {
        _plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;
    }

    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
        
    }

    private void UpdateVisual()
    {

    }
}
