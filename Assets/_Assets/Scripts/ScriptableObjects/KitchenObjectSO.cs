using UnityEngine;

[CreateAssetMenu(menuName = "Kitchen Object")]
public class KitchenObjectSO : ScriptableObject
{
    [field: SerializeField] public Transform ObjectPrefab { get; private set; }
    [field: SerializeField] public Sprite ObjectSprite { get; private set; }
    [field: SerializeField] public string ObjectName { get; private set; }
}
