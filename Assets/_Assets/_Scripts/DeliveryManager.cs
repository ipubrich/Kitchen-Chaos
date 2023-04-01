using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class DeliveryManager : NetworkBehaviour
{
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    // audio events
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailed;

    public static DeliveryManager Instance
    {
        get; private set;
    }
    //import SO to access lists
    [SerializeField] private RecipeListSO recipeListSO;
    
    // queue
    private List<RecipeSO> waitingRecipeList;
    // timer
    private float spawnRecipeTimer = 4f;
    private float spawnRecipeTimerMax = 4f;
    // max
    private int waitingRecipesMax = 4;
    // track number of recipes
    private int successfulRecipesAmount;



    private void Awake()
    {
        Instance = this;
        // init
        waitingRecipeList = new List<RecipeSO>();
    }
    private void Update()
    {
        // Only the server should create recipes
        if (!IsServer)
        {
            return;
        }

        // Create recipe
        spawnRecipeTimer -= Time.deltaTime;
        if (GameManager.Instance.IsGamePlaying() && spawnRecipeTimer <= 0f)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;

            // Check max items in queue
            if (GameManager.Instance.IsGamePlaying() && waitingRecipeList.Count < waitingRecipesMax)
            {
                int waitingRecipeSOIndex = UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count);
                SpawnNewWaitingRecipeClientRpc(waitingRecipeSOIndex); // RPC call to ensure clients sync with server
             
            }
        }
    }

    [ClientRpc]
    private void SpawnNewWaitingRecipeClientRpc(int waitingRecipeSOIndex)
    {
        // client local call
        RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[waitingRecipeSOIndex];

        waitingRecipeList.Add(waitingRecipeSO); // recipe generated

        OnRecipeSpawned?.Invoke(this, EventArgs.Empty);

    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        // check if items on platekitchenobject match order
        for (int i=0; i < waitingRecipeList.Count; i ++)
        {
            RecipeSO waitingRecipeSO = waitingRecipeList[i];

            if (waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
            {
                // Has same number of ingredients
                bool plateContentsMatchesRecipe = true;
                foreach (KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList)
                {
                    // cycling through all ingredients in the recipe
                    bool ingredientFound = false;
                    foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
                    {
                        // cycling through all ingredients on the plate  
                        if (plateKitchenObjectSO == recipeKitchenObjectSO)
                        {
                            // Ingredient Matches!
                            ingredientFound = true;
                            break;
                        }

                    }
                    if (!ingredientFound)
                    {
                        // This recipe ingredient was not found on the plate
                        plateContentsMatchesRecipe = false;

                    }
                }
                if (plateContentsMatchesRecipe)
                {
                    // Player delivered the correct recipe! Server update Rpc.
                    DeliverCorrectRecipeServerRpc(i);
                    return;
                }
            }
        }
        DeliverIncorrectRecipeServerRpc(); 
    }

    // Network incorrect delivery
    [ServerRpc(RequireOwnership = false)] // clients that dont own the network object will be allowed to trigger server rpc
    private void DeliverIncorrectRecipeServerRpc()
    {
        DeliverIncorrectRecipeClientRpc();

    }
    [ClientRpc]
    private void DeliverIncorrectRecipeClientRpc()
    {
        // Player did not deliver a correct recipe
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);

    }

    // Network successful delivery
    [ServerRpc(RequireOwnership = false)]  // clients that dont own the network object will be allowed to trigger server rpc
    private void DeliverCorrectRecipeServerRpc(int waitingRecipeSOListIndex) 
    {
        DeliverCorrectRecipeClientRpc(waitingRecipeSOListIndex); // server confirms delivery to all clients
    }

    [ClientRpc]
    private void DeliverCorrectRecipeClientRpc(int waitingRecipeSOListIndex)
    {
        successfulRecipesAmount++;
        waitingRecipeList.RemoveAt(waitingRecipeSOListIndex);
        OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
        OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
    }
    public List<RecipeSO> GetWaitingRecipeSOList()
    {
        return waitingRecipeList;
    }
    public int GetSuccessfulRecipesAmount()
    {
        return successfulRecipesAmount;
    }
}
