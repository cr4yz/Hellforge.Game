using System.Collections;
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
            _affixNameText.text = affixName + ":" + tier + ":" + roll;
        }

    }
}
