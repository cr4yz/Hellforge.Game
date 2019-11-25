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
                item.ExplicitAffixes.Remove(affix);
                GameObject.Destroy(gameObject);
            });

            _rollSlider.onValueChanged.AddListener((float value) =>
            {
                affix.Roll = (int)value;
                UpdateDescription(item, affix);
            });

            _tierSlider.onValueChanged.AddListener((float value) =>
            {
                affix.Tier = (int)value;
                UpdateDescription(item, affix);
            });

            UpdateDescription(item, affix);
        }

        private void UpdateDescription(Item item, ItemAffix affix)
        {
            var affixData = D4Data.Instance.Hellforge.GameData.Affixes.First(x => x.Name == affix.Name);
            _rollSlider.minValue = 0;
            _rollSlider.maxValue = 100;
            _rollSlider.wholeNumbers = true;
            _tierSlider.minValue = 0;
            _tierSlider.maxValue = affixData.Data.Length - 1;
            _tierSlider.wholeNumbers = true;
            _rollSlider.value = affix.Roll;
            _tierSlider.value = affix.Tier;
            _affixNameText.text = affixData.ParseDescription(affix.Tier, affix.Roll);
        }

    }
}
