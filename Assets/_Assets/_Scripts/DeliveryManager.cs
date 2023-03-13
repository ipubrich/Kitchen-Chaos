using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
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
    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;
    // max
    private int waitingRecipesMax = 4;

    private void Awake()
    {
        Instance = this;
        // init
        waitingRecipeList = new List<RecipeSO>();
    }
    private void Update()
    {
        // Create recipe
        spawnRecipeTimer -= Time.deltaTime;
        if (spawnRecipeTimer <= 0f)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;

            // Check max items in queue
            if (waitingRecipeList.Count < waitingRecipesMax)
            {
                RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)];
                waitingRecipeList.Add(waitingRecipeSO);

                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
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
                    // Player delivered the correct recipe!
                    waitingRecipeList.RemoveAt(i);
                    OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                    OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
                    return;
                }
            }
        }
        // No Matches found!
        // Player did not deliver a correct recipe
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);

    }

    public List<RecipeSO> GetWaitingRecipeSOList()
    {
        return waitingRecipeList;
    }
}
