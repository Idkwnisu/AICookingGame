using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueManagerUI : MonoBehaviour
{
    VisualElement root;
    VisualElement rootDialogue;
    public UIDocument uiDocument;

    Label characterSpeakingLabel;
    Label characterDialogueLabel;
    List<Button> playerChoicesButtons;

    void Awake()
    {
        GetUIReferences();
        ResetDialogueUI();
        HideUI();
        RegisterCallbacks();
        CreateArrays();
    }

    public void GetUIReferences()
    {
        root = uiDocument.rootVisualElement;
        rootDialogue = root.Q<VisualElement>("DialogueUI");

        characterSpeakingLabel = root.Q<Label>("CharacterSpeakingLabel");
        characterDialogueLabel = root.Q<Label>("NPCDialogue");

        playerChoicesButtons = new List<Button>();
        playerChoicesButtons.Add(root.Q<Button>("PlayerResponse1"));
        playerChoicesButtons.Add(root.Q<Button>("PlayerResponse2"));
        playerChoicesButtons.Add(root.Q<Button>("PlayerResponse3"));
    }

    public void ResetDialogueUI()
    {
        characterSpeakingLabel.text = "";

        characterDialogueLabel.text = "";
        characterDialogueLabel.SetEnabled(false);

        for(int i = 0; i < playerChoicesButtons.Count; i++)
        {
            playerChoicesButtons[i].text = "";
            playerChoicesButtons[i].SetEnabled(false);
        }
    }

    public void HideUI()
    {
        rootDialogue.visible = false;
        rootDialogue.SetEnabled(false);
    }

    public void ShowUI()
    {
        rootDialogue.visible = true;
        rootDialogue.SetEnabled(true);
    }

    public void RegisterCallbacks()
    {

    }

    public void CreateArrays()
    {

    }

    public void ShowEvent(DialogueEvent dialogue)
    {
        if(dialogue.isPlayer)
        {
            characterSpeakingLabel.text = "You";
            characterDialogueLabel.text = "";
            characterDialogueLabel.SetEnabled(false);

            for(int i = 0; i < playerChoicesButtons.Count; i++)
            {
                if(i < dialogue.playerDialogue.options.Count)
                {
                    playerChoicesButtons[i].text = dialogue.playerDialogue.options[i];
                    playerChoicesButtons[i].SetEnabled(true);
                }
                else
                {
                    playerChoicesButtons[i].text = "";
                    playerChoicesButtons[i].SetEnabled(false);
                }
            }
        }
        else
        {
            characterSpeakingLabel.text = dialogue.dialogueNPC.characterThatIsSpeaking.nomePersonaggio;
            characterDialogueLabel.text = dialogue.dialogueNPC.message;
            characterDialogueLabel.SetEnabled(true);

            for (int i = 0; i < playerChoicesButtons.Count; i++)
            {
                playerChoicesButtons[i].text = "";
                playerChoicesButtons[i].SetEnabled(false);
            }
        }
    }
}
