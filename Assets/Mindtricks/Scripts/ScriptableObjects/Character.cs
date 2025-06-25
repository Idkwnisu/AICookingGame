using UnityEngine;

public enum Emotion { NORMAL, ANGRY, SAD, SUSPICIOUS }
[CreateAssetMenu(fileName = "Character_", menuName = "Scriptable Objects/Character")]
public class Character : ScriptableObject
{
    public string nomePersonaggio;
    public string personalita;
    public Sprite[] immaginiEmozion;
}
