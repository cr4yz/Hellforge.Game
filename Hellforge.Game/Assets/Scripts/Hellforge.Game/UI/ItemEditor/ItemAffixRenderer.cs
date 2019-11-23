using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hellforge.Core.Items;

namespace Hellforge.Game.UI
{
    public class ItemAffixRenderer : MonoBehaviour
    {

        [SerializeField]
        private Text _affixNameText;
        public Button DeleteButton;

        public void Render(string affixName, int tier, int roll)
        {
            var desc = D4Data.Instance.Hellforge.GameData.Affixes.First(x => x.Name == affixName).ParseDescription(tier, roll);
            //_affixNameText.text = affixName + ":" + tier + ":" + roll;
            _affixNameText.text = desc;
        }

    }
}
