using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class IngredientProxy : MonoBehaviour
{
    public Text text;
    public Toggle toggle;
    public Ingredient ingredient;
    public bool selected;
    public int index;

    public UnityEvent<bool> changedValue;
    
    public void Init(Ingredient ing, int i)
    {
        ingredient = ing;
        index = i;
        text.text = ing.nomeIngrediente;
    }

    public void Select()
    {
        selected = true;
    }


    public void UnSelect()
    {
        selected = true;
    }
    public void ToggleSelect()
    {
        selected = !selected;
    }

    public void Disable()
    {
        toggle.interactable = false;
    }

    public void Enable()
    {
        toggle.interactable = true;
    }

    public void ToggleChanged(bool value)
    {
        selected = value;
        changedValue.Invoke(value);

    }
}
