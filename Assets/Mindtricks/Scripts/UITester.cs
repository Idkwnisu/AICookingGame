using UnityEngine;

public class UITester : MonoBehaviour
{
    public Ingredient[] ingredientsToTestWith;

    public ShopManager shopManager;

    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 150, 100), "Test Shop Manager"))
        {
            TestShopManager();
        }
    }

    public void TestShopManager()
    {
        shopManager.InitShop();
        shopManager.ShowUI();
    }

    public void DebugShopAction()
    {

    }
}
