using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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
    public CurrentTimeInfos currentTimeInfos;
    public Phase[] phasesOfDay;

    public UnityEvent<int> dayStarted;
    public IngredientManager ingredientManager;
    public RequestManager requestManager;
    public DialogueEventManager dialogueEventManager;

    private void Start()
    {
    }

    public void StartGame()
    {
        //Check for savefile here
        currentTimeInfos.currentDay = 0;
        currentTimeInfos.currentPhase = DAY_PHASE.NIGHT;

        NewDay();
    }


    public void NewDay()
    {
        currentTimeInfos.currentDay++;
        DeactivateCurrentPhaseGameObjects();
        phasesOfDay[phasesOfDay.Length - 1].onExit.Invoke();
        UnlockNewStuff();
        currentTimeInfos.currentPhase = phasesOfDay[0].phase;
        ActivateCurrentPhaseGameObjects();
        phasesOfDay[0].onEnter.Invoke();
    }

    public void DeactivateCurrentPhaseGameObjects()
    {
        for (int i = 0; i < phasesOfDay[(int)currentTimeInfos.currentPhase].toActivate.Length; i++)
        {
            phasesOfDay[(int)currentTimeInfos.currentPhase].toActivate[i].SetActive(false);
        }
    }

    public void ActivateCurrentPhaseGameObjects()
    {
        for (int i = 0; i < phasesOfDay[(int)currentTimeInfos.currentPhase].toActivate.Length; i++)
        {
            phasesOfDay[(int)currentTimeInfos.currentPhase].toActivate[i].SetActive(true);
        }
    }

    public void NewPhase()
    {
        if ((int)currentTimeInfos.currentPhase + 1 >= phasesOfDay.Length)
        {
            NewDay();
        }
        else
        {
            DeactivateCurrentPhaseGameObjects();
            phasesOfDay[((int)currentTimeInfos.currentPhase)].onExit.Invoke();
            currentTimeInfos.currentPhase++;
            phasesOfDay[((int)currentTimeInfos.currentPhase)].onEnter.Invoke();
            ActivateCurrentPhaseGameObjects();
        }
    }


    public void UnlockNewStuff()
    {
        ingredientManager.UnlockAllNewIngredients();
        requestManager.UnlockAllNewRequests();
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
