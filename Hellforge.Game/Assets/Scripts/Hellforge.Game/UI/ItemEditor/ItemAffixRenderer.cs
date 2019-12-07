using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hellforge.Core.Affixes;
using Hellforge.Core.Items;

namespace Hellforge.Game.UI
{
    public class ItemAffixRenderer : MonoBehaviour
    {

        [SerializeField]
        private Text _affixNameText;
        [SerializeField]
        private Slider _tierSlider;
        [SerializeField]
        private Slider _rollSlider;
        [SerializeField]
        private Button _deleteButton;

        public void Render(Item item, ItemAffix affix)
        {
            _deleteButton.onClick.AddListener(() =>
            {
                item.RemoveExplicitAffix(affix);
                GameObject.Destroy(gameObject);
            });

            _rollSlider.onValueChanged.AddListener((float value) =>
            {
                affix.Roll = (int)value;
                item.Dirty = true;
                UpdateDescription(item, affix);
            });

            _tierSlider.onValueChanged.AddListener((float value) =>
            {
                affix.Tier = (int)value;
                item.Dirty = true;
                UpdateDescription(item, affix);
            });

            UpdateDescription(item, affix);
        }

        private void UpdateDescription(Item item, ItemAffix affix)
        {
            var affixData = D4Data.Instance.Hellforge.GameData.Affixes.FirstOrDefault(x => x.Name == affix.Name);
            if(affixData == null)
            {
                _affixNameText.text = $"!{affix.Name}";
                _rollSlider.enabled = false;
                _tierSlider.enabled = false;
                return;
            }
            _rollSlider.minValue = 0;
            _rollSlider.maxValue = 100;
            _rollSlider.wholeNumbers = true;
            _tierSlider.minValue = 0;
            _tierSlider.maxValue = affixData.Data.Length - 1;
            _tierSlider.wholeNumbers = true;
            _rollSlider.value = affix.Roll;
            _tierSlider.value = affix.Tier;

            var desc = affixData.ParseDescription(affix.Tier, affix.Roll);
            var dumbAfx = item.Character.Hellforge.GenerateAffix(item.Character, affix.Name, affix.Tier, affix.Roll);

            foreach (var node in dumbAfx.Nodes)
            {
                if(node is ScriptedCondition cond)
                {
                    var color = cond.Passes() ? "#95FF00" : "red";
                    desc += $"\n<color={color}>{cond.GetDescription()}</color>";
                }
            }

            _affixNameText.text = desc;
        }

    }
}
