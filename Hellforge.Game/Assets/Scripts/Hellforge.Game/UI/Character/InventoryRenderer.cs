using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Hellforge.Game.World;
using Hellforge.Core.Items;

namespace Hellforge.Game.UI
{
    public class InventoryRenderer : MonoBehaviour
    {

        [SerializeField]
        private ItemRenderer _itemRendererTemplate;
        [SerializeField]
        private ItemEditorRenderer _itemEditor;
        private List<ItemRenderer> _itemRenderers = new List<ItemRenderer>();

        private void Awake()
        {
            _itemRendererTemplate.gameObject.SetActive(false);

            if (GameWorld.Instance.Character != null)
            {
                var classTree = D4Data.Instance.Hellforge.GameData.TalentTrees.First(x => x.Class == GameWorld.Instance.Character.Class);
                Render();
            }
        }

        public void CreateNewItem()
        {
            _itemEditor.gameObject.SetActive(true);

            var newItem = new Item("Treads", 0, 100);
            GameWorld.Instance.Character.AddItem(newItem);

            Render();

            _itemEditor.gameObject.SetActive(true);
            _itemEditor.Render(newItem);
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
                _itemRenderers.Add(clone);
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

