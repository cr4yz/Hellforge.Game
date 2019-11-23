using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hellforge.Core.Entities;

namespace Hellforge.Game.UI
{
    public class AffixCategoryRenderer : MonoBehaviour
    {
        [SerializeField]
        private Text _categoryText;
        [SerializeField]
        private AffixRenderer _affixTemplate;

        private void Start()
        {
            _affixTemplate.gameObject.SetActive(false);
        }

        public void Render(Character character, string category, List<string> affixes)
        {
            _categoryText.text = category;
            foreach (var affix in affixes)
            {
                var amount = character.GetAttribute(affix);
                var clone = GameObject.Instantiate(_affixTemplate, _affixTemplate.transform.parent);
                clone.Render(affix, amount);
                clone.gameObject.SetActive(true);
                clone.transform.SetAsLastSibling();
            }

        }

    }
}

