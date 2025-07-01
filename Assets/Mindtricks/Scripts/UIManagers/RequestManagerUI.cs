using System;
using System.Collections.Generic;
using System.Linq;
using Unity.AppUI.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;

public class RequestManagerUI : MonoBehaviour
{
    VisualElement root;
    VisualElement rootRequest;
    VisualElement characterVisuals;
    public UIDocument uiDocument;
    public Button templateButton;
    public VisualElement templateRow;
    private UnityEngine.UIElements.Button sendButton;
    private Label requestLabel;
    private Label answerLabel;
    private Label recipeLabel;
    private ScrollView scrollView;
    public Dictionary<Ingredient, Button> ingredientsSelected;
    public Dictionary<Ingredient, Button> allIngredients;

    public Color normalColor;
    public Color selectedColor;
    public Color blockedColor;
    public int numSelection = 5;

    private int rowSize = 5;
    private int columnSize = 12;

    public event Action sendingIngredients;
    public event Action refresh;

    public RequestManager requestManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        GetUIReferences();
        HideUI();
        RegisterCallbacks();
        CreateArrays();
    }

    public void GetUIReferences()
    {
        root = uiDocument.rootVisualElement;
        rootRequest = root.Q<VisualElement>("RequestUI");
        characterVisuals = root.Q<VisualElement>("CharacterVisuals");

        requestLabel = root.Q<Label>("RequestLabel_Requests");
        answerLabel = root.Q<Label>("AnswerLabel_Requests");
        recipeLabel = root.Q<Label>("RecipeLabel_Requests");
        scrollView = root.Q<ScrollView>("Ingredients_Requests");
        sendButton = root.Q<UnityEngine.UIElements.Button>("SendButton_Requests");
    }


    public void RegisterCallbacks()
    {
        sendButton.RegisterCallback<ClickEvent>(ClickedButton);

    }

    public void CreateArrays()
    {
        ingredientsSelected = new Dictionary<Ingredient, Button>();
        allIngredients = new Dictionary<Ingredient, Button>();
    }


    public void HideUI()
    {
        rootRequest.visible = false;
        rootRequest.SetEnabled(false);
        ResetUIAndIngredients();
    }

    public void ShowUI()
    {
        rootRequest.visible = true;
        rootRequest.SetEnabled(true);
    }


    private void ClickedButton(ClickEvent evt)
    {
        if (sendButton.text == "Send")
        {
            sendingIngredients?.Invoke();
            sendButton.text = "Go On";
        }
        else
        {
            refresh?.Invoke();
            sendButton.text = "Send";
        }
    }
    public void CreateUI(List<Ingredient> ingredientsToAdd)
    {
        ResetUIAndIngredients();
        for (int i = 0; i*rowSize < ingredientsToAdd.Count; i += 1)
        {
            VisualElement itemRoot = root.Q<VisualElement>("IngredientsRow" + (i+1) + "_Requests");
            itemRoot.ShowAndEnable();
            for (int j = 0; j < rowSize; j++)
            {
                if (i * rowSize + j < ingredientsToAdd.Count)
                {
                    Button button = itemRoot.Q<Button>("IngredientButton" + (i+1) + (j+1) + "_Requests");
                    button.ShowAndEnable();
                    button.text = ingredientsToAdd[i * rowSize + j].nomeIngrediente;

                    button.RegisterCallback<ClickEvent, Ingredient>(ClickEvent, ingredientsToAdd[i* rowSize + j]);
                    allIngredients.Add(ingredientsToAdd[i * rowSize + j], button);  
                }
            }
        }

    }

    public void ShowRequest(Request request, Character characterToShow)
    {
        requestLabel.text = request.requestText;
        characterVisuals.style.backgroundImage = new StyleBackground(characterToShow.immaginiEmozion[0]);
    }

    public void ResetUIAndIngredients()
    {
        characterVisuals.style.backgroundImage = new StyleBackground();
        allIngredients?.Clear();
        ingredientsSelected?.Clear();
        for (int i = 0; i < columnSize; i++)
        {
            VisualElement itemRoot = root.Q<VisualElement>("IngredientsRow" + (i + 1) + "_Requests");
            itemRoot.HideAndDisable();
            for (int j = 0; j < rowSize; j++)
            {
                Button button = itemRoot.Q<Button>("IngredientButton" + (i + 1) + (j + 1) + "_Requests");
                button.style.backgroundColor = normalColor;
                button.HideAndDisable();
            }
        }
        recipeLabel.text = "";
        answerLabel.text = "";
    }

    public void ClickEvent(ClickEvent ce, Ingredient button)
    {
        ClickButton(button);
    }

    public void HideSendButton()
    {
        sendButton.HideAndDisable();
    }

    public void ShowSendButton()
    {
        sendButton.ShowAndEnable();
    }

    public void ClickButton(Ingredient i)
    {
        Button button = allIngredients[i];
        if (ingredientsSelected.ContainsKey(i))
        {
            RemoveIngredientFromSelection(i);
        }
        else if(ingredientsSelected.Count < numSelection)
        {
            AddIngredientToSelection(i, button);
        }


        if (ingredientsSelected.Count >= numSelection)
        {
            UpdateColorOfButtonsForSelectionFull();
        }
        else
        {
            UpdateColorOfButtonsForSelectionNotFull();
        }
    }


    public void UpdateColorOfButtonsForSelectionFull()
    {
        foreach (KeyValuePair<Ingredient, Button> entry in allIngredients)
        {
            if (!requestManager.isIngredientSelected(entry.Key))
            {
                entry.Value.style.backgroundColor = blockedColor;
            }
            else
            {
                entry.Value.style.backgroundColor = selectedColor;
            }
        }
    }

    public void UpdateColorOfButtonsForSelectionNotFull()
    {
        foreach (KeyValuePair<Ingredient, Button> entry in allIngredients)
        {
            if (!requestManager.isIngredientSelected(entry.Key))
            {
                entry.Value.style.backgroundColor = normalColor;
            }
            else
            {
                entry.Value.style.backgroundColor = selectedColor;
            }
        }
    }

    public void RemoveIngredientFromSelection(Ingredient i)
    {
        ingredientsSelected.Remove(i);
        requestManager.RemoveIngredientSelected(i);
        allIngredients[i].style.backgroundColor = normalColor;
    }

    public void AddIngredientToSelection(Ingredient i, Button button)
    {
        ingredientsSelected.Add(i, button);
        requestManager.AddIngredientSelected(i);
        button.style.backgroundColor = selectedColor;
    }

    public void UpdateRequestLabel(string text)
    {
        requestLabel.text = text;
    }

    public void UpdateAnswerLabel(string text)
    {
        answerLabel.text = text;
    }
    public void UpdateRecipeLabel(string text)
    {
        recipeLabel.text = text;
    }

}
