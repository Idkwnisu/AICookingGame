using UnityEngine;

[CreateAssetMenu(fileName = "Ingredient", menuName = "Scriptable Objects/Ingredient")]
public class Ingredient : ScriptableObject
{
    public int id;
    public string nomeIngrediente;
    public string descrizione;
    public int costo;
}
