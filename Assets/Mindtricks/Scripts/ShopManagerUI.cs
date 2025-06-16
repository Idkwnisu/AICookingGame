using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using UIButton = UnityEngine.UIElements.Button;
public class ShopManagerUI : MonoBehaviour
{
    public ShopManager shopManager;
    VisualElement root;
    VisualElement rootShop;
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
    public int numSelection = 1;


    private int rowSize = 5;
    private int columnSize = 12;

    private void Awake()
    {
        GetUIReferences();
        RegisterCallbacks();
        HideUI();
        CreateArrays();
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
        rootShop = root.Q<VisualElement>("ShopUI");

        scrollView = root.Q<ScrollView>("Ingredients_Shop");
        infoPanel = root.Q<VisualElement>("InfoPanel_Shop");

        moneyLabel = root.Q<Label>("MoneyLabel_Shop");
        IngredientNameLabel = root.Q<Label>("IngredientNameLabel_Shop");
        IngredientDescriptionLabel = root.Q<Label>("IngredientDescriptionLabel_Shop");
        IngredientCostLabel = root.Q<Label>("IngredientCostLabel_Shop");

        buyButton = root.Q<UIButton>("BuyButton_Shop");
        buyButtonModal = root.Q<UIButton>("BuyButtonModal_Shop");
        infoButton = root.Q<UIButton>("InfoButton_Shop");
        closeButton = root.Q<UIButton>("CloseButton_Shop");
    }

    public void HideUI()
    {
        rootShop.HideAndDisable();
        ResetUIAndIngredients();
    }

    public void ShowUI()
    {
        rootShop.ShowAndEnable();
    }

    public void CreateArrays()
    {
        allIngredients = new Dictionary<Ingredient, Button>();
    }

    public void CreateUI(List<Ingredient> ingredientsToAdd)
    {
        ResetUIAndIngredients();

        for (int i = 0; i * rowSize < ingredientsToAdd.Count; i += 1)
        {
            int currentIngredientRow = (i + 1);
            VisualElement itemRoot = root.Q<VisualElement>("IngredientsRow" + currentIngredientRow + "_Shop");
            itemRoot.ShowAndEnable();
            for (int j = 0; j < rowSize; j++)
            {
                if (i * rowSize + j < ingredientsToAdd.Count)
                {
                    int currentRow = (j + 1);
                    int currentItemInFullList = i * rowSize + j;
                    Button button = itemRoot.Q<Button>("IngredientButton" + currentIngredientRow + currentRow + "_Shop");
                    button.ShowAndEnable();
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
        foreach(KeyValuePair<Ingredient, Button> ingredientButton in allIngredients)
        {
            ingredientButton.Value.style.backgroundColor = normalColor;
        }
        ingredientSelectedButton.style.backgroundColor = selectedColor;
    }


    public void ResetUIAndIngredients()
    {
        allIngredients?.Clear();
        for (int i = 0; i < columnSize; i++)
        {
            int currentIngredientRow = (i + 1);
            VisualElement itemRoot = root.Q<VisualElement>("IngredientsRow" + currentIngredientRow + "_Shop");
            itemRoot.HideAndDisable();

            for (int j = 0; j < rowSize; j++)
            {
                int currentRow = (j + 1);
                Button button = itemRoot.Q<Button>("IngredientButton" + currentIngredientRow + currentRow + "_Shop");
                button.style.backgroundColor = normalColor;
                button.HideAndDisable();
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
