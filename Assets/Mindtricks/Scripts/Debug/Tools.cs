using UnityEngine;

public class Tools : MonoBehaviour
{
    public Ingredient[] ingredientsToSet;
    private void OnValidate()
    {
        SetAllIngredientsId();
    }

    public void SetAllIngredientsId()
    {
        for(int i = 0; i < ingredientsToSet.Length; i++)
        {
            ingredientsToSet[i].id = i;
        }
    }

}
