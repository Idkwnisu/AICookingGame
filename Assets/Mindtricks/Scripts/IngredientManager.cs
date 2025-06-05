using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public struct UnlockIngredients
{
    public List<Ingredient> ingredientsUnlocked;
    public List<Ingredient> ingredientsNeededToUnlock;
    public int numOfIngredientsNeededToUnlock;
}


public class IngredientManager : MonoBehaviour
{
    public List<Ingredient> currentIngredients;
    public List<Ingredient> ingredientsToBuy;
    public List<UnlockIngredients> ingredientsToUnlockInTheShop;

    public RequestManager requestManager;
    public ShopManager shopManager;

    public UnityEvent IngredientsHaveChanged_Event;

    public void UnlockAllNewIngredients()
    {
        for(int i = 0; i < ingredientsToUnlockInTheShop.Count; i++)
        {
            bool unlockable = true;
            if (ingredientsToUnlockInTheShop[i].numOfIngredientsNeededToUnlock < currentIngredients.Count)
                unlockable = false;
            for(int j = 0; j <ingredientsToUnlockInTheShop[i].ingredientsNeededToUnlock.Count; j++)
            {
                if(!currentIngredients.Contains(ingredientsToUnlockInTheShop[i].ingredientsNeededToUnlock[j]))
                {
                    unlockable = false;
                }
            }

            if (unlockable)
            {
                for(int j = 0; j < ingredientsToUnlockInTheShop[i].ingredientsUnlocked.Count; j++)
                {
                    UnlockIngredient(ingredientsToUnlockInTheShop[i].ingredientsUnlocked[j]);
                }
            }
        }

    }

    public void UnlockIngredient(Ingredient ingredientToUnlock)
    {
        if(ingredientsToBuy.Contains(ingredientToUnlock))
        {
            ingredientsToBuy.Remove(ingredientToUnlock);
            currentIngredients.Add(ingredientToUnlock);
            requestManager.ResetOrInitRequestManager();
            IngredientsHaveChanged_Event.Invoke();
        }
    }    
}
