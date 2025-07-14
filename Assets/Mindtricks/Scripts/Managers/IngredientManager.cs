using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class UnlockIngredients
{
    public string name;
    public int id;
    public List<Ingredient> ingredientsUnlocked;
    public List<Ingredient> ingredientsNeededToUnlock;
    public int numOfIngredientsNeededToUnlock;
    public bool unlocked;
}


public class IngredientManager : MonoBehaviour
{
    public List<Ingredient> currentIngredients;
    public List<Ingredient> ingredientsToBuy;
    public List<UnlockIngredients> ingredientsToUnlockInTheShop;

    public RequestManager requestManager;
    public ShopManager shopManager;

    public UnityEvent IngredientsHaveChanged_Event;

    private List<int> unlocked;
    private List<int> bought;

    private void Awake()
    {
        unlocked = new List<int>();
        bought = new List<int>();
    }

    public void UnlockAllNewIngredients()
    {
        for(int i = 0; i < ingredientsToUnlockInTheShop.Count; i++)
        {
            bool unlockable = true;
            if (ingredientsToUnlockInTheShop[i].numOfIngredientsNeededToUnlock > currentIngredients.Count)
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
        if (!ingredientsToUnlockInTheShop[index].unlocked)
        {
            ingredientsToUnlockInTheShop[index].unlocked = true;
            unlocked.Add(index);
            for (int j = 0; j < ingredientsToUnlockInTheShop[index].ingredientsUnlocked.Count; j++)
            {
                AddItemToShop(ingredientsToUnlockInTheShop[index].ingredientsUnlocked[j]);
            }
        }
    }

    public void AddItemToShop(Ingredient ingredientToUnlock)
    {
        ingredientsToBuy.Add(ingredientToUnlock);
    }
    public void BuyIngredientFromTheShop(Ingredient ingredientToUnlock)
    {
        if(ingredientsToBuy.Contains(ingredientToUnlock))
        {
            ingredientsToBuy.Remove(ingredientToUnlock);
            currentIngredients.Add(ingredientToUnlock);
            bought.Add(ingredientToUnlock.id);
            IngredientsHaveChanged_Event.Invoke();
        }
    }

    public void BuyAllIngredients(List<int> allIngredienst)
    {
        for(int i = ingredientsToUnlockInTheShop.Count - 1; i >= 0 ; i--)
        {
            if(allIngredienst.Contains(ingredientsToUnlockInTheShop[i].id))
            {
                BuyIngredientFromTheShop(ingredientsToBuy[i]);
            }
        }
    }


    public List<int> GetIngredientsUnlocked()
    {
        return unlocked;
    }

    public List<int> GetIngredientsBought()
    {
        return bought;
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
