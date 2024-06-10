using UnityEngine;
using System.Collections.Generic;
using System;

public class PlateCompleteVisual : MonoBehaviour
{
    [Serializable]
    public struct KitchenObjectSO_GameObjct
    {
        public KitchenObjectSO kitchenObjectSO;
        public GameObject gameObject;
    }

    [SerializeField] private PlateKitchenObject _plateKitchenObject;
    [SerializeField] private List<KitchenObjectSO_GameObjct> _kitchenObjectSOGameObjectList;

    private void Start()
    {
        _plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;

        foreach (KitchenObjectSO_GameObjct kitchnObjectSOGameObject in _kitchenObjectSOGameObjectList)
        {
            kitchnObjectSOGameObject.gameObject.SetActive(false);
        }
    }

    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
        foreach (KitchenObjectSO_GameObjct kitchnObjectSOGameObject in _kitchenObjectSOGameObjectList)
        {
            if(kitchnObjectSOGameObject.kitchenObjectSO == e.kitchenObjectSO)
            {
                kitchnObjectSOGameObject.gameObject.SetActive(true);
            }
        }
    }
}
