using UnityEngine;
using UnityEngine.UI;

public class PlateIconSingleUI : MonoBehaviour
{
    [SerializeField] private Image _image;

    public void SetKitchenObjectSO(KitchenObjectSO kitchenObjectSO)
    {
        _image.sprite = kitchenObjectSO.ObjectSprite;
    }
}
