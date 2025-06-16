using UnityEngine;
using UnityEngine.UIElements;

public static class VisualElementExtensions
{
    public static void HideAndDisable(this VisualElement visualElement)
    {
        visualElement.visible = false;
        visualElement.SetEnabled(false);
    }
    public static void ShowAndEnable(this VisualElement visualElement)
    {
        visualElement.visible = true;
        visualElement.SetEnabled(true);
    }
    
    public static void SetVisibilityAndEnable(this VisualElement visualElement, bool value)
    {
        visualElement.visible = value;
        visualElement.SetEnabled(value);
    }


}
