using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueEvent", menuName = "Scriptable Objects/NPC Dialogue Event")]
public class NPCDialogueEvent : BaseDialogue
{
    public Character characterThatIsSpeaking;
    public string message;
    public Emotion emotionToUse;
    public BaseDialogue nextDialogue;
}
