using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ToggleActive : MonoBehaviour
{
    public KeyCode ToggleKey = KeyCode.None;
    public GameObject Target;
    public bool DisableOnAwake;

    void Awake()
    {
        if(Target == null)
        {
            Target = gameObject;
        }

        if(DisableOnAwake)
        {
            Target.SetActive(false);
        }
    }

    private void Update()
    {
        var focusedObj = EventSystem.current.currentSelectedGameObject;
        if(focusedObj != null && focusedObj.GetComponent<InputField>() != null)
        {
            return;
        }
        if(Input.GetKeyDown(ToggleKey))
        {
            Toggle();
        }
    }

    public void Toggle()
    {
        if(Target == null)
        {
            Target = gameObject;
        }
        Target.SetActive(!Target.activeSelf);
    }
}
