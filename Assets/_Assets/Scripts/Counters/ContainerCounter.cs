using UnityEngine;
using System;
using Unity.Netcode;

public class ContainerCounter : BaseCounter
{
    [SerializeField] protected KitchenObjectSO _kitchenObjectSO;

    public event EventHandler OnPlayerGrabbedObject;

    public override void Interact(Player player)
    {
        if(!player.HasKitchenObject())
        {
            KitchenObject.SpawnKitchenObject(_kitchenObjectSO, player);

            InteractLogicServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void InteractLogicServerRpc()
    {
        InteractLogicClientRpc();
    }

    [ClientRpc]
    private void InteractLogicClientRpc()
    {
        OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
    }
}
