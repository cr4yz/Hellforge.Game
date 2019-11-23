using UnityEngine;
using UnityEngine.UI;

public static class GameObjectExtensions
{
    public static void RebuildLayout(this GameObject root)
    {
        var layouts = root.GetComponentsInChildren<LayoutGroup>(true);
        foreach (var layout in layouts)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(layout.GetComponent<RectTransform>());
        }
    }
}