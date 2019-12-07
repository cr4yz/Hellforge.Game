using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class HideIfClickedOutside : MonoBehaviour
{

    private RectTransform _rt;
    private bool _skipOne;

    private void DetectOutsideClick()
    {
        if(_skipOne)
        {
            _skipOne = false;
            return;
        }
        if (Input.GetMouseButtonDown(0) &&
            !RectTransformUtility.RectangleContainsScreenPoint(_rt, Input.mousePosition))
        {
            gameObject.SetActive(false);
        }
    }

    void OnEnable()
    {
        _rt = GetComponent<RectTransform>();
        _skipOne = true;
    }

    void Update()
    {
        DetectOutsideClick();
    }
}
