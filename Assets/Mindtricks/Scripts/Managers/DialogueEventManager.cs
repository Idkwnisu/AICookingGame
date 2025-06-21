using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.Events;

public enum UnlockableDialogueType { STORY, RANDOM, ENDLESS, NIGHT }

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
    public List<DialogueEvent> regularDialoguesToDrawFrom;
    public List<DialogueEvent> storyDialoguesToDrawFrom;
    public List<DialogueEvent> nightDialoguesToDrawFrom;
    public List<DialogueEvent> endlessDialogues;
    public List<DialogueEvent> completedDialogues;
    public List<UnlockableDialogue> unlockableDialogues;
    public IngredientManager ingredientManager;
    public UnityEvent DialoguesAreOver;
    public float endlessProbability = .2f;

    public DialogueManagerUI dialogueManagerUI;

    private DialogueEvent currentEvent;

    private List<int> dialoguesUnlocked;
    private List<int> dialoguesRemoved;


    private void Start()
    {
        dialogueManagerUI.pressedEnemyGoOn += ClickedNPCGoOnButton;
        dialogueManagerUI.pressedCharacterChoice += ClickedPlayerChoiceButton;
    }


    internal void UnlockAndRemoveDialogues(List<int> dialoguesUnlocked, List<int> dialoguesRemoved)
    {
        for (int i = 0; i < unlockableDialogues.Count; i++)
        {
            if(dialoguesUnlocked.Contains(unlockableDialogues[i].unlockable.id))
            {
                UnlockDialogue(unlockableDialogues[i]);
                dialoguesUnlocked.Add(i);
            }
            if(dialoguesRemoved.Contains(unlockableDialogues[i].unlockable.id))
            {
                if(unlockableDialogues[i].type == UnlockableDialogueType.STORY)
                {
                    CompleteStoryDialogue(unlockableDialogues[i].unlockable);
                }
                else if(unlockableDialogues[i].type == UnlockableDialogueType.RANDOM)
                {
                    CompleteRegularDialogue(unlockableDialogues[i].unlockable);
                }
                else if(unlockableDialogues[i].type == UnlockableDialogueType.NIGHT)
                {
                    CompleteNightDialogue(unlockableDialogues[i].unlockable);
                }
                else
                {
                    Debug.LogError("ERROR: ENDLESS DIALOGUE IS COMPLETED, THIS IS NOT PERMITTED");
                }
            }
        }
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
        else if(regularDialoguesToDrawFrom.Count != 0 && UnityEngine.Random.Range(0.0f, 1.0f) > endlessProbability)
        {
            return regularDialoguesToDrawFrom[UnityEngine.Random.Range(0, regularDialoguesToDrawFrom.Count)];
        }
        else
        {
            return endlessDialogues[UnityEngine.Random.Range(0, endlessDialogues.Count)];
        }
    }

    public void ExtractNightDialogueAndStartIt()
    {
        if (nightDialoguesToDrawFrom.Count != 0)
        {
            StartDialogue(nightDialoguesToDrawFrom[UnityEngine.Random.Range(0, nightDialoguesToDrawFrom.Count)]);
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
                regularDialoguesToDrawFrom.Add(dialogue.unlockable);
                break;
            case UnlockableDialogueType.ENDLESS:
                endlessDialogues.Add(dialogue.unlockable);
                break;
            case UnlockableDialogueType.NIGHT:
                nightDialoguesToDrawFrom.Add(dialogue.unlockable);
                break;
        }
        dialoguesUnlocked.Add(dialogue.unlockable.id);
        regularDialoguesToDrawFrom.Add(dialogue.unlockable);
    }

    public void CompleteStoryDialogue(DialogueEvent dialogue)
    {
        completedDialogues.Add(dialogue);
        storyDialoguesToDrawFrom.Remove(dialogue);
        if(dialoguesUnlocked.Contains(dialogue.id))
        {
            dialoguesUnlocked.Remove(dialogue.id);
        }
        dialoguesRemoved.Add(dialogue.id);
    }
    public void CompleteRegularDialogue(DialogueEvent dialogue)
    {
        completedDialogues.Add(dialogue);
        regularDialoguesToDrawFrom.Remove(dialogue);
        if(dialoguesUnlocked.Contains(dialogue.id))
        {
            dialoguesUnlocked.Remove(dialogue.id);
        }
        dialoguesRemoved.Add(dialogue.id);
    }
    
    public void CompleteNightDialogue(DialogueEvent dialogue)
    {
        completedDialogues.Add(dialogue);
        nightDialoguesToDrawFrom.Remove(dialogue);
        if(dialoguesUnlocked.Contains(dialogue.id))
        {
            dialoguesUnlocked.Remove(dialogue.id);
        }
        dialoguesRemoved.Add(dialogue.id);
    }

    public List<int> GetDialoguesUnlocked()
    {
        return dialoguesUnlocked;
    }
    public List<int> GetDialoguesRemoved()
    {
        return dialoguesRemoved;
    }
}
