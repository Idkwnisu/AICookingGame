using System;
using UnityEngine;
using UnityEngine.UIElements;

public class SaveLoadButtonArgs : EventArgs
{
    public int buttonPressed { get; private set; }
    public bool loadButton { get; private set; }

    public SaveLoadButtonArgs(int buttonPressed, bool loadButton)
    {
        this.buttonPressed = buttonPressed;
        this.loadButton = loadButton;
    }
}

public class SaveFileManagerUI : MonoBehaviour
{
    VisualElement root;
    VisualElement rootSave;
    public UIDocument uiDocument;

    Button[] saveButton;
    bool[] isGamePresent;

    public event EventHandler<SaveLoadButtonArgs> saveButtonPressed;
    int numOfSaveFiles;

    public void Init(int numOfSaveFiles)
    {
        this.numOfSaveFiles = numOfSaveFiles;
        saveButton = new Button[numOfSaveFiles];
        isGamePresent = new bool[numOfSaveFiles];
        GetUIReferences();
        RegisterCallbacks();
    }

    public void GetUIReferences()
    {
        root = uiDocument.rootVisualElement;
        rootSave = root.Q<VisualElement>("LoadSaveUI");

        for(int i = 0; i < numOfSaveFiles; i++)
        {
            saveButton[i] = root.Q<Button>("SaveFileButton" + (i+1));
        }
    }


    public void HideUI()
    {
        rootSave.HideAndDisable();
    }

    public void ShowUI()
    {
        rootSave.ShowAndEnable();
    }

    public void RegisterCallbacks()
    {
        for (int i = 0; i < numOfSaveFiles; i++)
        {
            saveButton[i].RegisterCallback<ClickEvent, int>(ClickedSaveButton, i);
        }
    }

    public void ClickedSaveButton(ClickEvent ev, int button)
    {
        saveButtonPressed?.Invoke(this, new SaveLoadButtonArgs(button, isGamePresent[button]));
    }

    public void SetSaveFile(bool isPresent, int n, string date) //Insert playtime as well? Maybe a name?
    {
        isGamePresent[n] = isPresent;

        if (isPresent)
        {
            saveButton[n].text = $"Load game {n+1} - {date}";
        }
        else
        {
            saveButton[n].text = "New game";
        }
    }
}
