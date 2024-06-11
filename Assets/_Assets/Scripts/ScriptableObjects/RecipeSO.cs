using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Recipe")]
public class RecipeSO : ScriptableObject
{
    public List<KitchenObjectSO> kitchenObjectSOList;
    public string recipeName;
}
