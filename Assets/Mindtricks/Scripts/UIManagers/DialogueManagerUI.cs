using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class OptionSelectedArgs : EventArgs
{
    public int selected { get; private set; }
    public OptionSelectedArgs(int s)
    {
        selected = s;
    }
}


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

    public event EventHandler pressedEnemyGoOn;
    public event EventHandler<OptionSelectedArgs> pressedCharacterChoice;

    private int numOfPlayerChoicesInDialogue = 3;

    void Awake()
    {
        GetUIReferences();
        ResetDialogueUI();
        HideUI();
        RegisterCallbacks();
    }

    public void GetUIReferences()
    {
        root = uiDocument.rootVisualElement;
        rootDialogue = root.Q<VisualElement>("DialogueUI");
        characterImage = root.Q<VisualElement>("CharacterSpeaking");

        characterSpeakingLabel = root.Q<Label>("CharacterSpeakingLabel");
        characterDialogueLabel = root.Q<Label>("NPCDialogue");

        playerChoicesButtons = new List<Button>();
        for(int i = 1; i <= numOfPlayerChoicesInDialogue; i++)
        {
            playerChoicesButtons.Add(root.Q<Button>("PlayerResponse" + i));

        }

        NPCButtoGoOn = root.Q<Button>("NPCButtonGoOn");
    }

    public void ResetDialogueUI()
    {
        characterSpeakingLabel.text = "";

        characterDialogueLabel.text = "";
        characterDialogueLabel.HideAndDisable();
        NPCButtoGoOn.HideAndDisable();

        for (int i = 0; i < numOfPlayerChoicesInDialogue; i++)
        {
            playerChoicesButtons[i].text = "";
            playerChoicesButtons[i].HideAndDisable();
        }
    }

    public void HideUI()
    {
        rootDialogue.HideAndDisable();
    }

    public void ShowUI()
    {
        rootDialogue.ShowAndEnable();
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
        pressedEnemyGoOn?.Invoke(this, EventArgs.Empty);
    }

    public void ClickedPlayerButton(ClickEvent ev, int buttonNumber)
    {
        pressedCharacterChoice?.Invoke(this, new OptionSelectedArgs(buttonNumber));
    }

    public void ShowEvent(BaseDialogue dialogue)
    {
        if(dialogue is PlayerDialogueEvent playerDialogueEvent)
        {
            characterSpeakingLabel.text = "You";
            characterDialogueLabel.text = "";
            characterDialogueLabel.HideAndDisable();
            NPCButtoGoOn.HideAndDisable();

            for(int i = 0; i < playerChoicesButtons.Count; i++)
            {
                if(i < playerDialogueEvent.options.Count)
                {
                    playerChoicesButtons[i].text = playerDialogueEvent.options[i];
                    playerChoicesButtons[i].ShowAndEnable();
                }
                else
                {
                    playerChoicesButtons[i].text = "";
                    playerChoicesButtons[i].HideAndDisable();
                }
            }
        }
        else if(dialogue is NPCDialogueEvent npcDialogueEvent)
        {
            characterSpeakingLabel.text = npcDialogueEvent.characterThatIsSpeaking.nomePersonaggio;
            characterDialogueLabel.text = npcDialogueEvent.message;
            characterDialogueLabel.ShowAndEnable();
            NPCButtoGoOn.ShowAndEnable();

            characterImage.style.backgroundImage = new StyleBackground(npcDialogueEvent.characterThatIsSpeaking.immaginiEmozion[(int)npcDialogueEvent.emotionToUse]);

            for (int i = 0; i < playerChoicesButtons.Count; i++)
            {
                playerChoicesButtons[i].text = "";
                playerChoicesButtons[i].HideAndDisable();
            }
        }
    }
}
