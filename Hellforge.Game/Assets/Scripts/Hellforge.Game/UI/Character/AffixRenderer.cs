using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hellforge.Game.UI
{
    public class AffixRenderer : MonoBehaviour
    {

        [SerializeField]
        private Text _affixNameText;
        [SerializeField]
        private Text _affixAmountText;

        public void Render(string affixName, float amount)
        {
            _affixNameText.text = affixName;
            _affixAmountText.text = amount.ToString();
        }

    }
}

