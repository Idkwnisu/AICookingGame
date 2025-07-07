using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SaveFile
{
    public int currentMoney;
    public int currentDay;
    public int currentPhase;
    public List<int> ingredientsUnlocked;
    public List<int> dialoguesUnlocked;
    public List<int> dialoguesRemoved;
    public List<int> requestsUnlocked;
}


public class SaveFileManager : MonoBehaviour
{
    public FileDiskManager fileDiskManager;
    private SaveFile[] saveFiles;
    private int numberOfSaveFiles = 3;

    private int currentGameData;

    public IngredientManager ingredientManager;
    public RequestManager requestManager;
    public DialogueEventManager dialogueEventManager;
    public SaveFileManagerUI saveFileManagerUI;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        saveFiles = new SaveFile[numberOfSaveFiles];

        for(int i = 0; i < numberOfSaveFiles; i++)
        {
            if (PlayerPrefs.HasKey("SaveFile_" + i))
            {
                ReadFromDisk(i);
                saveFileManagerUI.SetSaveFile(true, i);
            }
            else
            {
                saveFileManagerUI.SetSaveFile(false, i);
                saveFiles[i] = new SaveFile();
                saveFiles[i].dialoguesUnlocked = new List<int>();
                saveFiles[i].ingredientsUnlocked = new List<int>();
                saveFiles[i].requestsUnlocked = new List<int>();
                saveFiles[i].currentDay = 1;
                saveFiles[i].currentPhase = 1;
            }
        }
    }

    public void DispatchButtonPressed(int pressed, bool loadGame)
    {
        if(loadGame)
        {
            LoadGameData(pressed);
        }
        else
        {
            CreateNewGameData(pressed);
        }
    }


    public void LoadGameData(int n)
    {
        if(saveFiles[n].currentDay == 1 && saveFiles[n].currentPhase == 1)
        {
            Debug.LogError("TRYING TO LOAD AN EMPTY GAME");
        }
        ingredientManager.UnlockIngredients(saveFiles[n].ingredientsUnlocked);
        requestManager.UnlockRequests(saveFiles[n].requestsUnlocked);
        dialogueEventManager.UnlockAndRemoveDialogues(saveFiles[n].dialoguesUnlocked, saveFiles[n].dialoguesRemoved);
    }

    public void SaveCurrentGameData()
    {
        saveFiles[currentGameData].ingredientsUnlocked = ingredientManager.GetIngredientsUnlocked();
        saveFiles[currentGameData].requestsUnlocked = requestManager.GetRequestsUnlocked();
        saveFiles[currentGameData].dialoguesUnlocked = dialogueEventManager.GetDialoguesUnlocked();
        saveFiles[currentGameData].dialoguesRemoved = dialogueEventManager.GetDialoguesRemoved();
    }

    public void CreateNewGameData(int n)
    {
        if(saveFiles[n].currentDay != 1 || saveFiles[n].currentPhase != 1)
        {
            Debug.LogError("TRYING TO CREATE A DATA ON NON EMPTY GAME");
        }
        currentGameData = n;
    }

    public void SaveToDisk(int n)
    {
        fileDiskManager.WriteToDisk("SaveFile_" + n, JsonUtility.ToJson(saveFiles[n]));
    }

    public void ReadFromDisk(int n)
    {
        saveFiles[n] = JsonUtility.FromJson<SaveFile>(fileDiskManager.ReadFromDisk("SaveFile_" + n));
    }
}
