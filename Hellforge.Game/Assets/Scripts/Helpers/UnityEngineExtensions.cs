using UnityEngine;
using UnityEngine.UI;

public static class UnityEngineExtensions
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

    public static void PositionTo(this RectTransform self, RectTransform target)
    {
        var prevParent = self.parent;
        self.SetParent(target, false);
        self.localScale = Vector3.one;
        self.anchoredPosition = Vector3.zero;
        self.SetParent(prevParent, true);
    }

    public static T GetOrAddComponent<T>(this GameObject obj)
        where T : MonoBehaviour
    {
        var result = obj.GetComponent<T>();
        if(result == null)
        {
            result = obj.AddComponent<T>();
        }
        return result;
    }
}