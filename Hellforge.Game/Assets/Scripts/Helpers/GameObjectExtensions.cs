using UnityEngine;
using UnityEngine.UI;

public static class GameObjectExtensions
{
    public static void RebuildLayout(this GameObject root)
    {
        var layouts = root.GetComponentsInChildren<LayoutGroup>(true);
        for(int i = layouts.Length - 1; i >= 0; i--)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(layouts[i].GetComponent<RectTransform>());
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(root.GetComponent<RectTransform>());
    }
}