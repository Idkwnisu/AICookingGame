using System;
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
    Button NPCButtoGoOn;
    VisualElement characterImage;

    public event Action pressedEnemyGoOn;
    public event Action<int> pressedCharacterChoice;

    private int numOfPlayerChoicesInDialogue = 3;

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
        characterImage = root.Q<VisualElement>("CharacterSpeaking");

        characterSpeakingLabel = root.Q<Label>("CharacterSpeakingLabel");
        characterDialogueLabel = root.Q<Label>("NPCDialogue");

        playerChoicesButtons = new List<Button>();
        for(int i = 0; i < numOfPlayerChoicesInDialogue; i++)
        {
            playerChoicesButtons.Add(root.Q<Button>("PlayerResponse" + i));

        }

        NPCButtoGoOn = root.Q<Button>("NPCButtoGoOn");
    }

    public void ResetDialogueUI()
    {
        characterSpeakingLabel.text = "";

        characterDialogueLabel.text = "";
        characterDialogueLabel.SetEnabled(false);

        for(int i = 0; i < numOfPlayerChoicesInDialogue; i++)
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
        NPCButtoGoOn.RegisterCallback<ClickEvent>(ClickedNPCButton);
        for (int i = 0; i < numOfPlayerChoicesInDialogue; i++)
        {
            playerChoicesButtons[i].RegisterCallback<ClickEvent, int>(ClickedPlayerButton, i);
        }
    }

    public void ClickedNPCButton(ClickEvent ev)
    {
        pressedEnemyGoOn.Invoke();
    }

    public void ClickedPlayerButton(ClickEvent ev, int buttonNumber)
    {
        pressedCharacterChoice.Invoke(buttonNumber);
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
            characterImage.style.backgroundImage = new StyleBackground(dialogue.dialogueNPC.characterThatIsSpeaking.immaginiEmozion[(int)dialogue.dialogueNPC.emotionToUse]);

            for (int i = 0; i < playerChoicesButtons.Count; i++)
            {
                playerChoicesButtons[i].text = "";
                playerChoicesButtons[i].SetEnabled(false);
            }
        }
    }
}
