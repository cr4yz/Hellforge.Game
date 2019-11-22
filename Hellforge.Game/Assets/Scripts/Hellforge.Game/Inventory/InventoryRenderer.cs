using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Hellforge.Game.World;

namespace Hellforge.Game.Inventory
{
    public class InventoryRenderer : MonoBehaviour
    {

        [SerializeField]
        private ItemRenderer _itemRendererTemplate;
        private List<ItemRenderer> _itemRenderers = new List<ItemRenderer>();

        private void Start()
        {
            _itemRendererTemplate.gameObject.SetActive(false);

            if (GameWorld.Instance.Character != null)
            {
                var classTree = D4Data.Instance.Hellforge.GameData.SkillTrees.First(x => x.Class == GameWorld.Instance.Character.Class);
                Render();
            }
        }

        private void Render()
        {
            ClearItems();

            var character = GameWorld.Instance.Character;

            foreach(var item in character.Items)
            {
                var clone = GameObject.Instantiate(_itemRendererTemplate, _itemRendererTemplate.transform.parent);
                clone.Render(item);
                clone.gameObject.SetActive(true);
            }
        }

        private void ClearItems()
        {
            foreach (var item in _itemRenderers)
            {
                GameObject.Destroy(item.gameObject);
            }
            _itemRenderers.Clear();
        }

    }
}

