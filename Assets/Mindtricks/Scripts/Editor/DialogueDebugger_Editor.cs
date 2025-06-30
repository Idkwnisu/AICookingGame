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
            CheckValidity((DialogueEvent)dialogueToDebug.objectReferenceValue);
            PrintDialogue((DialogueEvent)dialogueToDebug.objectReferenceValue, 0);
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

    public void PrintDialogue(DialogueEvent eventToPrint, int depth)
    {
        if (eventToPrint.isPlayer)
        {
            for (int i = 0; i < eventToPrint.playerDialogue.options.Count; i++)
            {
                
                debug += new string(' ', depth * 3);
                debug += eventToPrint.playerDialogue.options[i];
                debug += "\n";
                Debug.Log(debug);
                PrintDialogue(eventToPrint.playerDialogue.nextDialogues[i], depth+1);
            }
        }
        else
        {
            debug += new string(' ', depth * 3);
            debug += eventToPrint.dialogueNPC.message;
            debug += "\n";

            Debug.Log(debug);
            if (eventToPrint.dialogueNPC.nextDialogue != null)
            {
                PrintDialogue(eventToPrint.dialogueNPC.nextDialogue, depth+1);
            }
        }
    }

    public void CheckValidity(DialogueEvent eventToCheck)
    {
        if(eventToCheck.isPlayer)
        {
            if(eventToCheck.dialogueNPC.message != "")
            {
                Debug.LogError("A message in an event is not empty when not used - " + eventToCheck.name);
            }
            if(eventToCheck.playerDialogue.options.Count != eventToCheck.playerDialogue.nextDialogues.Count)
            {
                Debug.LogError("Mismatch in number of options and dialogues - " + eventToCheck.name);
            }
            for(int i = 0; i < eventToCheck.playerDialogue.options.Count; i++)
            {
                CheckValidity(eventToCheck.playerDialogue.nextDialogues[i]);
            }
        }
        else
        {
            if(eventToCheck.playerDialogue.options.Count != 0)
            {
                Debug.LogError("An NPC event has player options - " + eventToCheck.name);

            }
            if(eventToCheck.dialogueNPC.nextDialogue != null)
            {
                CheckValidity(eventToCheck.dialogueNPC.nextDialogue);
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
