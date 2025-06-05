using System;
using UnityEngine;
using UnityEngine.Events;

public enum DAY_PHASE
{
    MORNING,
    LUNCH,
    AFTERNOOON,
    DINNER,
    NIGHT
}

[Serializable]
public struct Phase {
    public DAY_PHASE phase;
    public UnityEvent onEnter;
    public UnityEvent onExit;
    public GameObject[] toActivate;
}

public class DayManager : MonoBehaviour
{
    public int currentDay;
    public DAY_PHASE currentPhase;
    public Phase[] phasesOfDay;

    public UnityEvent<int> dayStarted;
    public IngredientManager ingredientManager;
    public RequestManager requestManager;
    public DialogueEventManager dialogueEventManager;


    public void NewDay()
    {
        currentDay++;
        DeactivateCurrentPhaseGameObjects();
        phasesOfDay[phasesOfDay.Length - 1].onExit.Invoke();
        currentPhase = phasesOfDay[0].phase;
        ActivateCurrentPhaseGameObjects();
        phasesOfDay[0].onEnter.Invoke();
    }

    public void DeactivateCurrentPhaseGameObjects()
    {
        for (int i = 0; i < phasesOfDay[(int)currentPhase].toActivate.Length; i++)
        {
            phasesOfDay[(int)currentPhase].toActivate[i].SetActive(false);
        }
    }

    public void ActivateCurrentPhaseGameObjects()
    {
        for (int i = 0; i < phasesOfDay[(int)currentPhase].toActivate.Length; i++)
        {
            phasesOfDay[(int)currentPhase].toActivate[i].SetActive(true);
        }
    }

    public void NewPhase()
    {
        if ((int)currentPhase + 1 >= phasesOfDay.Length)
        {
            NewDay();
        }
        else
        {
            DeactivateCurrentPhaseGameObjects();
            currentPhase++;
            ActivateCurrentPhaseGameObjects();
        }
    }


    public void UnlockNewStuff()
    {
        requestManager.UnlockAllNewRequests();
        ingredientManager.UnlockAllNewIngredients();
        dialogueEventManager.UnlockAllNewDialogueEvents();
    }

    public bool IsDialogueUnlockable(UnlockableDialogue unlockableDialogue)
    {
        if(ingredientManager.currentIngredients.Count < unlockableDialogue.numOfIngredientsNeededToUnlock)
        {
            return false;
        }
        else
        {
            for(int i = 0; i < unlockableDialogue.ingredientsNeededToUnlock.Count; i++)
            {
                if (!ingredientManager.currentIngredients.Contains(unlockableDialogue.ingredientsNeededToUnlock[i]))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
