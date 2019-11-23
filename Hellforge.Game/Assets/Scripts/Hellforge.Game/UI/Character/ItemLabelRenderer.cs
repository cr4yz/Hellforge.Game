using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Hellforge.Core.Items;

namespace Hellforge.Game.UI
{
    public class ItemLabelRenderer : MonoBehaviour
    {

        [SerializeField]
        private Text _nameText;
        [SerializeField]
        private Text _slotText;
        [SerializeField]
        private Text _affixesText;

        public void Render(Item item)
        {
            var data = item.Character.Hellforge.GameData.ItemBases.First(x => x.Name == item.BaseName);
            _nameText.text = data.Name;
            _slotText.text = data.Slot;

            var affixText = string.Empty;

            foreach(var afx in data.ImplicitAffixes)
            {
                var affix = item.Character.Hellforge.GameData.Affixes.First(x => x.Name == afx.Name);
                var desc = affix.ParseDescription(afx.Tier, item.BaseRoll);
                affixText += desc + "\n";
            }

            foreach(var afx in item.ExplicitAffixes)
            {
                var affix = item.Character.Hellforge.GameData.Affixes.First(x => x.Name == afx.Name);
                var desc = affix.ParseDescription(afx.Tier, afx.Roll);
                affixText += desc + "\n";
            }

            _affixesText.text = affixText.TrimEnd('\r', '\n');
        }

    }
}

