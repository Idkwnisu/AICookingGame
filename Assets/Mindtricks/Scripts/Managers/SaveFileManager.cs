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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        saveFiles = new SaveFile[numberOfSaveFiles];
        for(int i = 0; i < numberOfSaveFiles; i++)
        {
            if (PlayerPrefs.HasKey("SaveFile_" + i))
            {
                ReadFromDisk(i);
            }
            else
            {
                saveFiles[i] = new SaveFile();
                saveFiles[i].dialoguesUnlocked = new List<int>();
                saveFiles[i].ingredientsUnlocked = new List<int>();
                saveFiles[i].requestsUnlocked = new List<int>();
            }
        }
    }

    public void LoadGameData(int n)
    {
        ingredientManager.UnlockIngredients(saveFiles[currentGameData].ingredientsUnlocked);
        requestManager.UnlockRequests(saveFiles[currentGameData].requestsUnlocked);
        dialogueEventManager.UnlockAndRemoveDialogues(saveFiles[currentGameData].dialoguesUnlocked, saveFiles[currentGameData].dialoguesRemoved);
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
