using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Request", menuName = "Scriptable Objects/Request")]
public class Request : ScriptableObject
{
    public int id;
    public string requestText;
    public int payment;
    public List<Ingredient> ingredientsNeededToUnlock;
    public int numOfIngredientsNeededToUnlock;
}
