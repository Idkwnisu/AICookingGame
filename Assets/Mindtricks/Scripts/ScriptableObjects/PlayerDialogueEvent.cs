using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "DialogueEvent", menuName = "Scriptable Objects/Player Dialogue Event")]
public class PlayerDialogueEvent : BaseDialogue
{
    public List<string> options;
    public List<BaseDialogue> nextDialogues;
}
