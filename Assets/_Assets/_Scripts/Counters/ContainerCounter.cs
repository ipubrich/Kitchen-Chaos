using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    public event EventHandler OnPlayerGrabbedObject;
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            // Check player is not carrying anything before giving object
            KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);
           
            // fire event for ContainerCounterVisual
            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        }
    }
}
