using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hellforge.Core.Entities;
using Hellforge.Game.World;

namespace Hellforge.Game.UI
{
	public class StatSheetRenderer : MonoBehaviour
	{

        [SerializeField]
        private AffixCategoryRenderer _categoryRendererTemplate;
        private Dictionary<string, List<string>> _affixCategories = new Dictionary<string, List<string>>();
        private List<AffixCategoryRenderer> _categoryRenderers = new List<AffixCategoryRenderer>();

        private void Start()
        {
            _categoryRendererTemplate.gameObject.SetActive(false);

            foreach (var attr in D4Data.Instance.Hellforge.GameData.Attributes)
            {
                if (!_affixCategories.ContainsKey(attr.Category))
                {
                    _affixCategories.Add(attr.Category, new List<string>());
                }
                _affixCategories[attr.Category].Add(attr.Name);
            }

            if(GameWorld.Instance.Character != null)
            {
                Render(GameWorld.Instance.Character);
            }
        }

        public void Render(Character character)
        {
            Wipe();

            foreach(var kvp in _affixCategories)
            {
                var clone = GameObject.Instantiate(_categoryRendererTemplate, _categoryRendererTemplate.transform.parent);
                clone.Render(character, kvp.Key, kvp.Value);
                clone.gameObject.SetActive(true);
                _categoryRenderers.Add(clone);
            }

            gameObject.RebuildLayout();
        }

        private void Wipe()
        {
            foreach(var catRenderer in _categoryRenderers)
            {
                GameObject.Destroy(catRenderer.gameObject);
            }
            _categoryRenderers.Clear();
        }

	}
}

