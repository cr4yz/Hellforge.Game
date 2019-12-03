using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

namespace Hellforge.Game.UI
{
    public class AffixBuilder : MonoBehaviour
    {

        [SerializeField]
        private AffixEditor _affixEditor;
        [SerializeField]
        private InputField _identifierInput;
        [SerializeField]
        private InputField _inheritsInput;
        [SerializeField]
        private Dropdown _typeDropdown;
        [SerializeField]
        private Toggle _forTalentToggle;
        [SerializeField]
        private Dropdown _attributeDropdown;
        [SerializeField]
        private InputField _invokeInput;
        [SerializeField]
        private Dropdown _itemSlotDropdown;
        [SerializeField]
        private Dropdown _affixSlotDropdown;
        [SerializeField]
        private InputField _descriptionInput;
        [SerializeField]
        private InputField _conditionsInput;
        [SerializeField]
        private InputField _activatorsInput;
        [SerializeField]
        private InputField _tierDataInput;
        [SerializeField]
        private Button _saveButton;
        [SerializeField]
        private Button _deleteButton;
        [SerializeField]
        private Button _duplicateButton;

        private AffixEntry _renderedAffix;

        public void Render(AffixEntry affix)
        {
            _renderedAffix = affix;
            _identifierInput.text = affix.Name;
            _inheritsInput.text = affix.Inherits;
            _typeDropdown.value = _typeDropdown.options.FindIndex(x => x.text == affix.Type);
            _attributeDropdown.value = _attributeDropdown.options.FindIndex(x => x.text == affix.Attribute);
            _invokeInput.text = affix.Invoke;
            _itemSlotDropdown.value = _itemSlotDropdown.options.FindIndex(x => x.text == affix.ItemSlot);
            _affixSlotDropdown.value = _affixSlotDropdown.options.FindIndex(x => x.text == affix.Slot);
            _descriptionInput.text = affix.Description;
            _conditionsInput.text = ArrToLsv(affix.Conditions);
            _activatorsInput.text = ArrToLsv(affix.Activators);
            _tierDataInput.text = TiersToText(affix.Data);
            _forTalentToggle.isOn = affix.ForTalent;

            _saveButton.onClick.RemoveAllListeners();
            _deleteButton.onClick.RemoveAllListeners();
            _duplicateButton.onClick.RemoveAllListeners();

            _saveButton.onClick.AddListener(delegate ()
            {
                UpdateRenderedAffix();
                _affixEditor.Render();
            });

            _deleteButton.onClick.AddListener(delegate ()
            {
                _affixEditor.RemoveAffix(affix);
                _affixEditor.Render();
                _renderedAffix = null;
                gameObject.SetActive(false);
            });

            _duplicateButton.onClick.AddListener(delegate ()
            {
                var duplicate = InputToAffixEntry();
                duplicate.Name += " (DUPLICATE)";
                _affixEditor.AddAffix(duplicate);
                Render(duplicate);
            });
        }

        private void UpdateRenderedAffix()
        {
            _renderedAffix.Name = _identifierInput.text;
            _renderedAffix.Inherits = _inheritsInput.text;
            _renderedAffix.Type = _typeDropdown.options[_typeDropdown.value].text;
            _renderedAffix.Attribute = _attributeDropdown.options[_attributeDropdown.value].text;
            _renderedAffix.Invoke = _invokeInput.text;
            _renderedAffix.ItemSlot = _itemSlotDropdown.options[_itemSlotDropdown.value].text;
            _renderedAffix.Slot = _affixSlotDropdown.options[_affixSlotDropdown.value].text;
            _renderedAffix.Description = _descriptionInput.text;
            _renderedAffix.Conditions = LsvToArr(_conditionsInput.text);
            _renderedAffix.Activators = LsvToArr(_activatorsInput.text);
            _renderedAffix.Data = TextToTiers(_tierDataInput.text);
            _renderedAffix.ForTalent = _forTalentToggle.isOn;
        }

        private AffixEntry InputToAffixEntry()
        {
            return new AffixEntry()
            {
                Name = _identifierInput.text,
                Inherits = _inheritsInput.text,
                Type = _typeDropdown.options[_typeDropdown.value].text,
                Attribute = _attributeDropdown.options[_attributeDropdown.value].text,
                Invoke = _invokeInput.text,
                ItemSlot = _itemSlotDropdown.options[_itemSlotDropdown.value].text,
                Slot = _affixSlotDropdown.options[_affixSlotDropdown.value].text,
                Description = _descriptionInput.text,
                Conditions = LsvToArr(_conditionsInput.text),
                Activators = LsvToArr(_conditionsInput.text),
                Data = TextToTiers(_tierDataInput.text),
                ForTalent = _forTalentToggle.isOn
            };
        }

        private string ArrToLsv(string[] input)
        {
            var result = string.Empty;
            if(input == null)
            {
                return result;
            }
            foreach(var x in input)
            {
                result += x + '\n';
            }
            return result.TrimEnd('\n');
        }

        private string TiersToText(AffixDataEntry[] data)
        {
            var result = string.Empty;
            if (data == null)
            {
                return result;
            }
            foreach (var tier in data)
            {
                //result += $"(name:{tier.Name}) (roll:{tier.Minimum}-{tier.Maximum}) (duration:{tier.Duration}) (amount:{tier.Amount})\n";
                result += $"name={tier.Name},roll={tier.Minimum}-{tier.Maximum},duration={tier.Duration},amount={tier.Amount}\n";
            }
            return result.TrimEnd('\n');
        }

        private string[] LsvToArr(string input)
        {
            if(string.IsNullOrEmpty(input))
            {
                return null;
            }
            return input.Split('\n');
        }

        private AffixDataEntry[] TextToTiers(string input)
        {
            if(string.IsNullOrEmpty(input))
            {
                return null;
            }
            var result = new List<AffixDataEntry>();
            var regex = new Regex(@"([^=|^,|^\W]+)=([^,|^\*]+)");
            var lines = input.Split('\n');
            foreach(var line in lines)
            {
                var entry = new AffixDataEntry();
                foreach (Match match in regex.Matches(line))
                {
                    var key = match.Groups[1].Value.ToLower();
                    var value = match.Groups[2].Value;
                    switch (key)
                    {
                        case "name":
                            entry.Name = value;
                            break;
                        case "amount":
                            entry.Amount = int.Parse(value);
                            break;
                        case "roll":
                            var split = value.Split('-');
                            entry.Minimum = int.Parse(split[0]);
                            entry.Maximum = int.Parse(split[1]);
                            break;
                        case "duration":
                            entry.Duration = float.Parse(value);
                            break;
                    }
                }
                result.Add(entry);
            }
            return result.ToArray();
        }

    }
}

