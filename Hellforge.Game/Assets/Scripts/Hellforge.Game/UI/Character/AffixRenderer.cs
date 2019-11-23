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

        public string AffixName { get; private set; }
        public float AffixAmount { get; private set; } = -1.0344f;

        public void Render(string affixName, float amount)
        {
            _affixNameText.text = affixName;
            _affixAmountText.text = amount.ToString();

            AffixName = affixName;
            AffixAmount = amount;
        }

    }
}

