using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using UIButton = UnityEngine.UIElements.Button;
public class ShopManagerUI : MonoBehaviour
{
    public ShopManager shopManager;
    VisualElement root;
    public UIDocument uiDocument;
    public VisualElement templateRow;

    public Button ingredientSelectedButton;
    private UIButton buyButton;
    private UIButton buyButtonModal;
    private UIButton infoButton;
    private UIButton closeButton;
    private Label moneyLabel;

    private VisualElement infoPanel;
    private Label IngredientNameLabel;
    private Label IngredientDescriptionLabel;
    private Label IngredientCostLabel;
    public Dictionary<Ingredient, UIButton> allIngredients;


    private ScrollView scrollView;
    public Color normalColor;
    public Color selectedColor;
    public Color blockedColor;
    public int numSelection = 1;


    private int rowSize = 5;
    private int columnSize = 12;

    private void Start()
    {
        GetUIReferences();
        RegisterCallbacks();

        allIngredients = new Dictionary<Ingredient, Button>();        
}

    public void RegisterCallbacks()
    {
        buyButton.RegisterCallback<ClickEvent>(BuyButtonClicked);
        buyButtonModal.RegisterCallback<ClickEvent>(BuyButtonClicked);
        infoButton.RegisterCallback<ClickEvent>(InfoButtonClicked);
        closeButton.RegisterCallback<ClickEvent>(CloseButtonClicked);
    }

    public void GetUIReferences()
    {
        root = uiDocument.rootVisualElement;

        scrollView = root.Q<ScrollView>("Ingredients");
        infoPanel = root.Q<VisualElement>("InfoPanel");

        moneyLabel = root.Q<Label>("MoneyLabel");
        IngredientNameLabel = root.Q<Label>("IngredientNameLabel");
        IngredientDescriptionLabel = root.Q<Label>("IngredientDescriptionLabel");
        IngredientCostLabel = root.Q<Label>("IngredientCostLabel");

        buyButton = root.Q<UIButton>("BuyButton");
        buyButtonModal = root.Q<UIButton>("BuyButtonModal");
        infoButton = root.Q<UIButton>("InfoButton");
        closeButton = root.Q<UIButton>("CloseButton");
    }

    public void CreateUI(List<Ingredient> ingredientsToAdd)
    {
        ResetUI();

        for (int i = 0; i * rowSize < ingredientsToAdd.Count; i += 1)
        {
            int currentIngredientRow = (i + 1);
            VisualElement itemRoot = root.Q<VisualElement>("IngredientsRow" + currentIngredientRow);
            itemRoot.visible = true;
            for (int j = 0; j < rowSize; j++)
            {
                if (i * rowSize + j < ingredientsToAdd.Count)
                {
                    int currentRow = (j + 1);
                    int currentItemInFullList = i * rowSize + j;
                    Button button = itemRoot.Q<Button>("IngredientButton" + currentIngredientRow + currentRow);
                    button.visible = true;
                    button.text = ingredientsToAdd[currentItemInFullList].nomeIngrediente;

                    button.RegisterCallback<ClickEvent, Ingredient>(ClickEvent, ingredientsToAdd[currentItemInFullList]);
                    allIngredients.Add(ingredientsToAdd[currentItemInFullList], button);
                }
            }
        }

    }

    public void ClickEvent(ClickEvent ce, Ingredient button)
    {
        SelectIncredientClickedButton(button);
    }

    //TO DO, al posto di selezionare una lista, cambia la selezione da quello precedente a questo
    public void SelectIncredientClickedButton(Ingredient i)
    {
        shopManager.SelectIngredient(i);
        ingredientSelectedButton = allIngredients[i];
        ingredientSelectedButton.style.backgroundColor = selectedColor;
    }


    public void ResetUI()
    {
        for (int i = 0; i < columnSize; i++)
        {
            int currentIngredientRow = (i + 1);
            VisualElement itemRoot = root.Q<VisualElement>("IngredientsRow" + currentIngredientRow);

            for (int j = 0; j < rowSize; j++)
            {
                int currentRow = (j + 1);
                Button button = itemRoot.Q<Button>("IngredientButton" + currentIngredientRow + currentRow);
                button.visible = false;
            }
        }
    }


    private void BuyButtonClicked(ClickEvent evt)
    {
        shopManager.BuyCurrentIngredient();

    }
    private void InfoButtonClicked(ClickEvent evt)
    {
        ShowInfoModal(shopManager.GetIngredientSelected());   
    }

    private void ShowInfoModal(Ingredient i)
    {
        infoPanel.visible = true;
        IngredientNameLabel.text = i.name;
        IngredientDescriptionLabel.text = i.descrizione;
        IngredientCostLabel.text = i.costo.ToString();
    }

    private void CloseButtonClicked(ClickEvent evt)
    {
        CloseInfoModal();
    }

    private void CloseInfoModal()
    {
        infoPanel.visible = false;
        IngredientNameLabel.text = "";
        IngredientDescriptionLabel.text = "";
        IngredientCostLabel.text = "";
    }
}
