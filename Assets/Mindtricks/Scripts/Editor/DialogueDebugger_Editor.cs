using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEditor;

[CustomEditor(typeof(DialogueDebugger))]
public class DialogueDebugger_Editor : Editor
{
    SerializedProperty toDebug;

    private string debug = "";

    private void OnEnable()
    {
        toDebug = serializedObject.FindProperty("toDebug");
    }

    public override void OnInspectorGUI()
    {
        // Update the serialized object
        serializedObject.Update();

        EditorGUILayout.LabelField("Dialogue Debugger");
        // Draw the property field

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(toDebug);
        if(EditorGUI.EndChangeCheck())
        {
            debug = "";
            CheckValidity((DialogueEvent)toDebug.objectReferenceValue);
            PrintDialogue((DialogueEvent)toDebug.objectReferenceValue, 0);
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
}
