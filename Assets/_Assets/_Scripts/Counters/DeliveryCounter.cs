using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter {


    // singleton for audio
    public static DeliveryCounter Instance { get; private set; }


    private void Awake() {
        Instance = this;
    }


    public override void Interact(Player player) {
        if (player.HasKitchenObject()) {
            if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) {
                // Only accepts Plates

                // Deliver plate with recipe
                DeliveryManager.Instance.DeliverRecipe(plateKitchenObject);

                // Cleanup
                KitchenObject.DestroyKitchenObject(plateKitchenObject);
                
            }
        }
    }

}