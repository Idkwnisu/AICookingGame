using UnityEngine;

public class UITester : MonoBehaviour
{
    public ShopManager shopManager;
    public DayManager dayManager;
    public DialogueEventManager dialogueManager;

    public DialogueEvent dialogueToTest;

    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 150, 100), "Test Shop Manager"))
        {
            TestShopManager();
        }

        if (GUI.Button(new Rect(600, 10, 150, 100), "Next Phase"))
        {
            NextPhase();
        }
        
        if (GUI.Button(new Rect(600, 160, 150, 100), "Next Day"))
        {
            NextDay();
        }
        
        if (GUI.Button(new Rect(1000, 160, 150, 100), "Test Single Dialogue"))
        {
            TestDialogue();
        }

    }

    public void NextPhase()
    {
        dayManager.NewPhase();
    }

    public void NextDay()
    {
        dayManager.NewDay();

    }

    public void TestShopManager()
    {
        shopManager.OpenShopScreen();
        shopManager.ShowUI();
    }

    public void DebugShopAction()
    {

    }

    public void TestDialogue()
    {
        dialogueManager.OpenDialogueScreen();
        dialogueManager.StartDialogue(dialogueToTest);
    }
}
