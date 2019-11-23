using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hellforge.Core.Entities;
using Hellforge.Game.World;

namespace Hellforge.Game.UI
{
    public class AffixCategoryRenderer : MonoBehaviour
    {
        [SerializeField]
        private Text _categoryText;
        [SerializeField]
        private AffixRenderer _affixTemplate;

        private List<AffixRenderer> _affixRenderers = new List<AffixRenderer>();

        private void Start()
        {
            _affixTemplate.gameObject.SetActive(false);
        }

        private void Update()
        {
            if(GameWorld.Instance.Character != null)
            {
                foreach (var affixRenderer in _affixRenderers)
                {
                    var amount = GameWorld.Instance.Character.GetAttribute(affixRenderer.AffixName);
                    if(amount != affixRenderer.AffixAmount)
                    {
                        affixRenderer.Render(affixRenderer.AffixName, amount);
                    }
                }
            }
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
                _affixRenderers.Add(clone);
            }
        }

    }
}

