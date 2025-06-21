using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RequestManager : MonoBehaviour
{
    public List<Ingredient> IngredientsSelectedList;

    public IngredientManager ingredientManager;

    public MoneyManager moneyManager;

    public RequestManagerUI requestManagerUI;

    public RecipeSender recipeSender;


    public List<Request> requestsInRotation;
    public List<Request> requestsToUnlock;
    private Request currentRequest;

    private int requestsBeforeGoingOn = 3;

    public UnityEvent requestsOver;
    private int scoreForFullPayment = 7;

    private void Awake()
    {
        IngredientsSelectedList = new List<Ingredient>();
        requestManagerUI.sendingIngredients += SendFullRecipe;
        requestManagerUI.refresh += RefreshUI;
        requestManagerUI.refresh += ExtractNewRequest;
    }

    public void RefreshRequestsBeforeGoingOn()
    {
        //TODO: Random formula based on day here(with a cap)
        requestsBeforeGoingOn = 3;
    }

    public void SendFullRecipe()
    {
        requestManagerUI.HideSendButton();
        recipeSender.SendRecipe(IngredientsSelectedList, currentRequest.requestText);
    }

    public void RefreshUI()
    {
        //Check here if it's done for the day
        requestManagerUI.ResetUIAndIngredients();
        requestManagerUI.CreateUI(ingredientManager.currentIngredients);
    }

    public void ExtractNewRequest()
    {
        if (requestsBeforeGoingOn > 0)
            ExtractRequestAndExecuteIt();
        else
            requestsOver.Invoke();
    }

    internal void ShowUI()
    {
        requestManagerUI.ShowUI();
    }
    internal void HideUI()
    {
        requestManagerUI.HideUI();
    }
    public void ShowRequestUI()
    {
        RefreshUI();
        requestManagerUI.CreateUI(ingredientManager.currentIngredients);
        ShowUI();
    }
    public void CloseRequestUI()
    {
        HideUI();
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

    public void HideSendButton()
    {
        requestManagerUI.HideSendButton();
    }

    public void ShowSendButton()
    {
        requestManagerUI.ShowSendButton();
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


    public Request ExtractRequest()
    {
        return requestsInRotation[UnityEngine.Random.Range(0, requestsInRotation.Count)];
    }

    public void ExtractRequestAndExecuteIt()
    {
        requestsBeforeGoingOn--;
        ExecuteRequest(ExtractRequest());
    }

    public void ExecuteRequest(Request request)
    {
        currentRequest = request;
        requestManagerUI.ShowRequest(request);
    }

    public void UpdateRecipe(string recipe)
    {
        requestManagerUI.UpdateRecipeLabel(recipe);
    }

    public void UpdateScore(string score)
    {
        if(int.Parse(score) > scoreForFullPayment)
        {
            moneyManager.EarnMoney(currentRequest.payment);
        }
        else
        {
            moneyManager.EarnMoney(currentRequest.payment / 2);
        }
    }
    public void UpdateScore(int score)
    {
        if(score > scoreForFullPayment)
        {
            moneyManager.EarnMoney(currentRequest.payment);
        }
        else
        {
            moneyManager.EarnMoney(currentRequest.payment / 2);
        }
    }

    public void UpdateStructuredScore(string score)
    {
        StructuredScore structuredScore = JsonUtility.FromJson<StructuredScore>(score);
        UpdateScore(structuredScore.score);
    }

    public void UpdateRequestAnswer(string answer)
    {
        requestManagerUI.UpdateAnswerLabel(answer);
    }

    public void UpdateStructuredRecipe(string recipe)
    {
        StructuredRecipe structuredRecipe = JsonUtility.FromJson<StructuredRecipe>(recipe);
        UpdateRecipe(structuredRecipe.recipeName + "\n" + structuredRecipe.recipeDescription);
    }

    public void UpdateStructuredRequestAnswer(string answer)
    {
        StructuredAnswer structuredAnswer = JsonUtility.FromJson<StructuredAnswer>(answer);
        UpdateRequestAnswer(structuredAnswer.answer);
    }
}
