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
    public void InitShop()
    {
        shopManagerUI.CreateUI(ingredientManager.ingredientsToBuy);
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
            shopManagerUI.CreateUI(ingredientManager.ingredientsToBuy);
        }
    
    }

    public Ingredient GetIngredientSelected()
    {
        return ingredientSelected;
    }
}
