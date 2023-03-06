using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCounter : BaseCounter
{
    public override void Interact(Player player)
    {
        // check if player is holding something to destroy
        if (player.HasKitchenObject())
        {
            player.GetKitchenObject().DestroySelf();
        }
        
    }

}
