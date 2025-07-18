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
    public List<int> ingredientsBought;
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
    public MoneyManager moneyManager;

    public DayManager dayManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        saveFiles = new SaveFile[numberOfSaveFiles];

        saveFileManagerUI.Init(numberOfSaveFiles);
        saveFileManagerUI.saveButtonPressed += DispatchButtonPressed;


        for (int i = 0; i < numberOfSaveFiles; i++)
        {

            if (PlayerPrefs.HasKey("SaveFile_" + i))
            {
                ReadFromDisk(i);
                saveFileManagerUI.SetSaveFile(true, i, PlayerPrefs.GetString("SaveFile_" + i));
            }
            else
            {
                saveFileManagerUI.SetSaveFile(false, i, "");
                saveFiles[i] = new SaveFile();
                saveFiles[i].dialoguesUnlocked = new List<int>();
                saveFiles[i].ingredientsUnlocked = new List<int>();
                saveFiles[i].requestsUnlocked = new List<int>();
                saveFiles[i].currentDay = 1;
                saveFiles[i].currentPhase = 1;
            }
        }
    }

    private void OnDestroy()
    {
        saveFileManagerUI.saveButtonPressed -= DispatchButtonPressed;

    }

    public void DispatchButtonPressed(object sender, SaveLoadButtonArgs args)
    {
        if(args.loadButton)
        {
            LoadGameData(args.buttonPressed);
        }
        else
        {
            CreateNewGameData(args.buttonPressed);
        }

        GoToGame();
    }

    public void GoToGame()
    {
        saveFileManagerUI.HideUI();
        dayManager.StartGame();
    }



    public void LoadGameData(int n)
    {
        if(saveFiles[n].currentDay == 1 && saveFiles[n].currentPhase == 1)
        {
            Debug.LogError("TRYING TO LOAD AN EMPTY GAME");
        }
        dayManager.SetDayAndPhase(saveFiles[n].currentDay, saveFiles[n].currentPhase);
        ingredientManager.UnlockIngredients(saveFiles[n].ingredientsUnlocked);
        ingredientManager.BuyAllIngredients(saveFiles[n].ingredientsBought);
        requestManager.UnlockRequests(saveFiles[n].requestsUnlocked);
        dialogueEventManager.UnlockAndRemoveDialogues(saveFiles[n].dialoguesUnlocked, saveFiles[n].dialoguesRemoved);
    }

    public void SaveCurrentGameData()
    {
        PlayerPrefs.SetString("SaveFile_" + currentGameData, DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());
        saveFiles[currentGameData].ingredientsUnlocked = ingredientManager.GetIngredientsUnlocked();
        saveFiles[currentGameData].ingredientsBought = ingredientManager.GetIngredientsBought();
        saveFiles[currentGameData].requestsUnlocked = requestManager.GetRequestsUnlocked();
        saveFiles[currentGameData].dialoguesUnlocked = dialogueEventManager.GetDialoguesUnlocked();
        saveFiles[currentGameData].dialoguesRemoved = dialogueEventManager.GetDialoguesRemoved();
        saveFiles[currentGameData].currentDay = dayManager.currentTimeInfos.currentDay;
        saveFiles[currentGameData].currentPhase = (int)dayManager.currentTimeInfos.currentPhase;
        saveFiles[currentGameData].currentMoney = moneyManager.currentFinancialinfos.currentMoney;

        SaveToDisk(currentGameData);
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
