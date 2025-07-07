using System;
using UnityEngine;
using UnityEngine.UIElements;

public class SaveFileManagerUI : MonoBehaviour
{
    VisualElement root;
    VisualElement rootSave;
    public UIDocument uiDocument;

    Button[] saveButton;
    bool[] isGamePresent;

    public event Action<int, bool> saveButtonPressed;
    int numOfSaveFiles;

    void Init(int numOfSaveFiles)
    {
        this.numOfSaveFiles = numOfSaveFiles;
        saveButton = new Button[numOfSaveFiles];
        isGamePresent = new bool[numOfSaveFiles];
        GetUIReferences();

    }

    public void GetUIReferences()
    {
        root = uiDocument.rootVisualElement;
        rootSave = root.Q<VisualElement>("LoadSaveUI");

        for(int i = 0; i < numOfSaveFiles; i++)
        {
            saveButton[i] = rootSave.Q<Button>("SaveFileButton" + i);
        }
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
        saveButtonPressed?.Invoke(button, isGamePresent[button]);
    }

    public void SetSaveFile(bool isPresent, int n) //Insert playtime as well? Maybe a name?
    {
        if(isPresent)
        {
            saveButton[n].text = "Load game 1";
        }
        else
        {
            saveButton[n].text = "New game";
        }
    }
}
