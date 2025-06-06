using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct UnlockableRequest
{
    public string unlockableRequest;
    public int numOfIngredientsNeededToUnlock;
    public List<Ingredient> ingredientsNeededToUnlock;
}


public class RequestManager : MonoBehaviour
{
    public List<Ingredient> IngredientsSelectedList;

    public IngredientManager ingredientManager;

    public RequestManagerUI requestManagerUI;


    public List<Request> requestsInRotation;
    public List<Request> requestsToUnlock;




    private void Awake()
    {
        IngredientsSelectedList = new List<Ingredient>();
    }

    internal void ShowUI()
    {
        requestManagerUI.ShowUI();
    }
    internal void HideUI()
    {
        requestManagerUI.HideUI();
    }
    public void ResetOrInitRequestManager()
    {
        requestManagerUI.CreateUI(ingredientManager.currentIngredients);
    }

    public bool isIngredientSelected(Ingredient ingredient)
    {
        if(IngredientsSelectedList.Contains(ingredient))
        {
            return true;
        }
        else
        {
            return false;
        }    
    }

    public bool isIngredientSelected(int i)
    {
        return isIngredientSelected(ingredientManager.currentIngredients[i]);
    }

    public bool getIngredient(int i)
    {
        return ingredientManager.currentIngredients[i];
    }

    public void RemoveIngredientSelected(Ingredient i)
    {
        IngredientsSelectedList.Remove(i);
    }
    
    public void AddIngredientSelected(Ingredient i)
    {
        IngredientsSelectedList.Add(i);
    }

    public void UnlockAllNewRequests()
    {
        for (int i = 0; i < requestsToUnlock.Count; i++)
        {
            if (IsRequestUnlockable(requestsToUnlock[i]))
            {
                UnlockRequest(requestsToUnlock[i]);
            }
        }
    }

    public bool IsRequestUnlockable(Request request)
    {
        if (ingredientManager.currentIngredients.Count < request.numOfIngredientsNeededToUnlock)
        {
            return false;
        }
        else
        {
            for (int i = 0; i < request.ingredientsNeededToUnlock.Count; i++)
            {
                if (!ingredientManager.currentIngredients.Contains(request.ingredientsNeededToUnlock[i]))
                {
                    return false;
                }
            }
            return true;
        }
    }


    public void UnlockRequest(Request request)
    {
        requestsToUnlock.Remove(request);
        requestsInRotation.Add(request);
    }

}
