using System;
using System.Collections.Generic;
using UnityEngine;



public enum DialogueSpeaker
{
    PLAYER,
    NPC
}

public interface BaseDialogue
{
    public DialogueSpeaker dialogueSpeaker { get; set; }
}

[Serializable]
public class NPCDialogue : BaseDialogue
{
    DialogueSpeaker BaseDialogue.dialogueSpeaker { get; set; }
    public Character characterThatIsSpeaking;
    public string message;
    public Emotion emotionToUse;
    public DialogueEvent nextDialogue;
}

[Serializable]
public class PlayerDialogue : BaseDialogue
{
    DialogueSpeaker BaseDialogue.dialogueSpeaker { get; set; }
    public List<string> options;
    public List<DialogueEvent> nextDialogues;
}

[CreateAssetMenu(fileName = "DialogueEvent", menuName = "Scriptable Objects/Dialogue Event")]
public class DialogueEvent : ScriptableObject
{
    public bool isPlayer;
    public NPCDialogue dialogueNPC;
    public PlayerDialogue playerDialogue;
}
