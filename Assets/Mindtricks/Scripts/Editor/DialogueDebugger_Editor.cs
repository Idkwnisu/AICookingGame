using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(Sanitizer))]
public class DialogueDebugger_Editor : Editor
{
    SerializedProperty dialogueToDebug;
    SerializedProperty requestsToDebug;

    private string debug = "";

    private void OnEnable()
    {
        dialogueToDebug = serializedObject.FindProperty("dialogueToDebug");
        requestsToDebug = serializedObject.FindProperty("requestsToDebug");
    }

    public override void OnInspectorGUI()
    {
        // Update the serialized object
        serializedObject.Update();

        EditorGUILayout.LabelField("Dialogue Debugger");
        // Draw the property field

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(dialogueToDebug);
        if(EditorGUI.EndChangeCheck())
        {
            debug = "";
            CheckValidity((BaseDialogue)dialogueToDebug.objectReferenceValue);
            PrintDialogue((BaseDialogue)dialogueToDebug.objectReferenceValue, 0);
        }
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(requestsToDebug);
        if(EditorGUI.EndChangeCheck())
        {
            CheckRequestIndexes(requestsToDebug);
        }

        EditorGUILayout.Separator();
        EditorGUILayout.HelpBox(debug, MessageType.Info, true);

        // Apply any changes
        serializedObject.ApplyModifiedProperties();
    }

    public void PrintDialogue(BaseDialogue eventToPrint, int depth)
    {
        if (eventToPrint is PlayerDialogueEvent playerDialogueEvent)
        {
            for (int i = 0; i < playerDialogueEvent.options.Count; i++)
            {
                
                debug += new string(' ', depth * 3);
                debug += playerDialogueEvent.options[i];
                debug += "\n";
                Debug.Log(debug);
                PrintDialogue(playerDialogueEvent.nextDialogues[i], depth+1);
            }
        }
        else if(eventToPrint is NPCDialogueEvent npcDialogueEvent)
        {
            debug += new string(' ', depth * 3);
            debug += npcDialogueEvent.message;
            debug += "\n";

            Debug.Log(debug);
            if (npcDialogueEvent.nextDialogue != null)
            {
                PrintDialogue(npcDialogueEvent.nextDialogue, depth+1);
            }
        }
    }

    public void CheckValidity(BaseDialogue eventToCheck)
    {
        if (eventToCheck is PlayerDialogueEvent playerDialogueEvent)
        {
            if (playerDialogueEvent.options.Count != playerDialogueEvent.nextDialogues.Count)
            {
                Debug.LogError("Mismatch in number of options and dialogues - " + eventToCheck.name);
            }
            for (int i = 0; i < playerDialogueEvent.options.Count; i++)
            {
                CheckValidity(playerDialogueEvent.nextDialogues[i]);
            }
        }
        else if(eventToCheck is NPCDialogueEvent npcDialogueEvent)
        { 
            if (npcDialogueEvent.nextDialogue != null)
            {
                CheckValidity(npcDialogueEvent.nextDialogue);
            }
        }
    }


    public void CheckRequestIndexes(SerializedProperty requestsToCheck)
    {
        HashSet<int> idHasSets = new HashSet<int>();
        for(int i = 0; i < requestsToCheck.arraySize; i++)
        {
            Request toCheck = ((Request)requestsToCheck.GetArrayElementAtIndex(i).objectReferenceValue);
            if (!idHasSets.Add((toCheck.id)))
            {
                Debug.LogError($"Duplicate ID at: {toCheck.name}");
            }
        }
    }
}
