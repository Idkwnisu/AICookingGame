using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class ShopManager : MonoBehaviour
{
    public Ingredient ingredientSelected;


    public List<Ingredient> IngredientsSelectedList;
    public IngredientManager ingredientManager;
    public MoneyManager moneyManager;
    public ShopManagerUI shopManagerUI;

    private void Start()
    {
        IngredientsSelectedList = new List<Ingredient>();
    }

    internal void ShowUI()
    {
        shopManagerUI.ShowUI();
    }
    internal void HideUI()
    {
        shopManagerUI.HideUI();
    }

    public void InitShopUI()
    {
        shopManagerUI.CreateUI(ingredientManager.ingredientsToBuy);
        ShowUI();
    }

    public void CloseShop()
    {
        HideUI();
    }

    internal void SelectIngredient(Ingredient i)
    {
        ingredientSelected = i;
    }

    internal void BuyCurrentIngredient()
    {
        if (moneyManager.isMoneyEnough(ingredientSelected.costo))
        {
            moneyManager.SpendMoney(ingredientSelected.costo);
            ingredientManager.UnlockIngredient(ingredientSelected);

            IngredientsSelectedList.Remove(ingredientSelected);
            ingredientSelected = null;

            shopManagerUI.CreateUI(ingredientManager.ingredientsToBuy);
        }
    
    }

    public Ingredient GetIngredientSelected()
    {
        return ingredientSelected;
    }
}
