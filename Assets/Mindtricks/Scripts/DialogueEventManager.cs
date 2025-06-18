using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Events;

public enum UnlockableDialogueType { STORY, RANDOM, ENDLESS }

[Serializable]
public struct UnlockableDialogue
{
    public DialogueEvent unlockable;
    public int numOfIngredientsNeededToUnlock;
    public List<Ingredient> ingredientsNeededToUnlock;
    public UnlockableDialogueType type;
}
[Serializable]
public struct SpecificDialogue
{
    public DialogueEvent unlockable;
    public int numberOfIngredientsToUnlock;
    public List<Ingredient> ingredientsNeededToUnlock;
}


public class DialogueEventManager : MonoBehaviour
{
    public List<DialogueEvent> startingDialoguesToDrawFrom;
    public List<DialogueEvent> storyDialoguesToDrawFrom;
    public List<DialogueEvent> endlessDialogues;
    public List<UnlockableDialogue> unlockableDialogues;
    public IngredientManager ingredientManager;
    public UnityEvent DialoguesAreOver;
    public float endlessProbability = .2f;

    public DialogueManagerUI dialogueManagerUI;

    private DialogueEvent currentEvent;


    private void Start()
    {
        dialogueManagerUI.pressedEnemyGoOn += ClickedNPCGoOnButton;
        dialogueManagerUI.pressedCharacterChoice += ClickedPlayerChoiceButton;
    }

    public void ClickedNPCGoOnButton()
    {
        if(currentEvent.dialogueNPC.nextDialogue != null)
        {
            StartDialogue(currentEvent.dialogueNPC.nextDialogue);
        }
        else
        {
            dialogueManagerUI.ResetDialogueUI();
            DialoguesAreOver.Invoke();
        }
    }

    public void ClickedPlayerChoiceButton(int buttonPressed)
    {
        if(currentEvent.playerDialogue.nextDialogues.Count != 0)
        {
            StartDialogue(currentEvent.playerDialogue.nextDialogues[buttonPressed]);
        }
        else
        {
            DialoguesAreOver.Invoke();
        }
    }

    public void OpenDialogueScreen()
    {
        dialogueManagerUI.ShowUI();
    }

    public void CloseDialogueScreen()
    {
        dialogueManagerUI.ResetDialogueUI();
        dialogueManagerUI.HideUI();
    }
    public DialogueEvent ExtractDialogue()
    {
        if(storyDialoguesToDrawFrom.Count != 0)
        {
            return storyDialoguesToDrawFrom[UnityEngine.Random.Range(0, storyDialoguesToDrawFrom.Count)];
        }
        else if(startingDialoguesToDrawFrom.Count != 0 && UnityEngine.Random.Range(0.0f, 1.0f) > endlessProbability)
        {
            return startingDialoguesToDrawFrom[UnityEngine.Random.Range(0, startingDialoguesToDrawFrom.Count)];
        }
        else
        {
            return endlessDialogues[UnityEngine.Random.Range(0, endlessDialogues.Count)];
        }
    }

    public void ExtractDialogueAndStartIt()
    {
        StartDialogue(ExtractDialogue());
    }

    public void StartDialogue(DialogueEvent dialogueEventToStart)
    {
        currentEvent = dialogueEventToStart;
        dialogueManagerUI.ShowEvent(dialogueEventToStart);
    }

    public bool IsDialogueUnlockable(UnlockableDialogue unlockableDialogue)
    {
        if (ingredientManager.currentIngredients.Count < unlockableDialogue.numOfIngredientsNeededToUnlock)
        {
            return false;
        }
        else
        {
            for (int i = 0; i < unlockableDialogue.ingredientsNeededToUnlock.Count; i++)
            {
                if (!ingredientManager.currentIngredients.Contains(unlockableDialogue.ingredientsNeededToUnlock[i]))
                {
                    return false;
                }
            }
            return true;
        }
    }

    public void UnlockAllNewDialogueEvents()
    {
        for (int i = 0; i < unlockableDialogues.Count; i++)
        {
            if (IsDialogueUnlockable(unlockableDialogues[i]))
            {
                UnlockDialogue(unlockableDialogues[i]);
            }
        }
    }

    public void UnlockDialogue(UnlockableDialogue dialogue)
    {
        unlockableDialogues.Remove(dialogue);
        switch(dialogue.type)
        {
            case UnlockableDialogueType.STORY:
                storyDialoguesToDrawFrom.Add(dialogue.unlockable);
                break;
            case UnlockableDialogueType.RANDOM:
                startingDialoguesToDrawFrom.Add(dialogue.unlockable);
                break;
            case UnlockableDialogueType.ENDLESS:
                endlessDialogues.Add(dialogue.unlockable);
                break;
        }
        startingDialoguesToDrawFrom.Add(dialogue.unlockable);
    }


}
