using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownFilter : MonoBehaviour
{
    [SerializeField]
    private InputField _inputField;
    [SerializeField]
    private bool _clearOnDisable;
    [SerializeField]
    private Dropdown _dropDown;

    private List<Dropdown.OptionData> _dropdownOptions;

    private void Start()
    {
        _dropdownOptions = _dropDown.options;
        _inputField.onValueChanged.AddListener(delegate (string value)
        {
            FilterDropdown(value);
        });
    }

    private void OnDisable()
    {
        if(_clearOnDisable)
        {
            _inputField.text = string.Empty;
        }
    }

    public void FilterDropdown(string input)
    {
        _dropDown.options = _dropdownOptions.FindAll(option => option.text.IndexOf(input, StringComparison.InvariantCultureIgnoreCase) != -1);
    }
}