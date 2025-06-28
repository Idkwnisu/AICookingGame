using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public struct UnlockIngredients
{
    public string name;
    public int id;
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

    public List<int> unlocked;

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
                UnlockAllIngredientsFromASingleUnlockable(i);
            }
        }

    }

    public void UnlockAllIngredientsFromASingleUnlockable(int index)
    {
        unlocked.Add(index);
        for (int j = 0; j < ingredientsToUnlockInTheShop[index].ingredientsUnlocked.Count; j++)
        {
            UnlockIngredient(ingredientsToUnlockInTheShop[index].ingredientsUnlocked[j]);
        }
    }

    public void UnlockIngredient(Ingredient ingredientToUnlock)
    {
        if(ingredientsToBuy.Contains(ingredientToUnlock))
        {
            ingredientsToBuy.Remove(ingredientToUnlock);
            currentIngredients.Add(ingredientToUnlock);
            IngredientsHaveChanged_Event.Invoke();
        }
    }


    public List<int> GetIngredientsUnlocked()
    {
        return unlocked;
    }

    public void UnlockIngredients(List<int> idIngredients)
    {
        for(int i = 0; i < ingredientsToUnlockInTheShop.Count; i++)
        {
            if(idIngredients.Contains(ingredientsToUnlockInTheShop[i].id))
            {
                UnlockAllIngredientsFromASingleUnlockable(ingredientsToUnlockInTheShop[i].id);
            }
        }
    }
}
