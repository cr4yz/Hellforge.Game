using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hellforge.Game.UI
{
    public class BaseItemEditor : MonoBehaviour
    {

        [SerializeField]
        private BaseItemBuilder _baseItemBuilder;
        [SerializeField]
        private Button _itemSlotBtnTemplate;
        [SerializeField]
        private Button _baseItemBtnTemplate;
        [SerializeField]
        private Button _createBaseItemButton;

        private string _selectedSlot;

        private List<GameObject> _itemSlotBtns = new List<GameObject>();
        private List<GameObject> _baseItemBtns = new List<GameObject>();

        void Awake()
        {
            _baseItemBtnTemplate.gameObject.SetActive(false);
            _itemSlotBtnTemplate.gameObject.SetActive(false);
            _createBaseItemButton.gameObject.SetActive(false);
            _baseItemBuilder.gameObject.SetActive(false);
        }

        void Start()
        {
            Render();

            _createBaseItemButton.onClick.AddListener(delegate ()
            {
                var newItem = new ItemBaseEntry()
                {
                    Name = "New Base Item",
                    InventoryHeight = 2,
                    InventoryWidth = 2,
                    Slot = _selectedSlot ?? "Ring",
                    Type = "Equipment"
                };
                AddBaseItem(newItem);
                _baseItemBuilder.gameObject.SetActive(true);
                _baseItemBuilder.Render(newItem);
            });
        }

        public void RemoveBaseItem(ItemBaseEntry baseItem)
        {
            var baseItems = D4Data.Instance.Hellforge.GameData.ItemBases.ToList();
            baseItems.Remove(baseItem);
            D4Data.Instance.Hellforge.GameData.ItemBases = baseItems.ToArray();
            Render();
        }

        public void AddBaseItem(ItemBaseEntry baseItem)
        {
            var baseItems = D4Data.Instance.Hellforge.GameData.ItemBases.ToList();
            baseItems.Add(baseItem);
            D4Data.Instance.Hellforge.GameData.ItemBases = baseItems.ToArray();
            Render();
        }

        public void Render()
        {
            Wipe();

            RenderItemSlots();
            RenderBaseItems(_selectedSlot);
        }

        private void RenderItemSlots()
        {
            foreach (var obj in _itemSlotBtns)
            {
                GameObject.Destroy(obj);
            }
            _itemSlotBtns.Clear();

            foreach (var slot in D4Data.Instance.Hellforge.GameData.ItemSlots)
            {
                var clone = GameObject.Instantiate(_itemSlotBtnTemplate, _itemSlotBtnTemplate.transform.parent);
                clone.GetComponentInChildren<Text>().text = slot;
                clone.onClick.AddListener(delegate ()
                {
                    _selectedSlot = slot;
                    _createBaseItemButton.gameObject.SetActive(true);
                    _baseItemBuilder.gameObject.SetActive(false);
                    RenderBaseItems(slot);
                });
                clone.gameObject.SetActive(true);
                _itemSlotBtns.Add(clone.gameObject);
            }
        }

        private void RenderBaseItems(string itemSlot)
        {
            foreach (var obj in _baseItemBtns)
            {
                GameObject.Destroy(obj);
            }
            _baseItemBtns.Clear();

            foreach(var baseItem in D4Data.Instance.Hellforge.GameData.ItemBases)
            {
                if(baseItem.Slot != itemSlot)
                {
                    continue;
                }
                var clone = GameObject.Instantiate(_baseItemBtnTemplate, _baseItemBtnTemplate.transform.parent);
                clone.GetComponentInChildren<Text>().text = baseItem.Name;
                clone.gameObject.SetActive(true);
                clone.onClick.AddListener(delegate ()
                {
                    _baseItemBuilder.gameObject.SetActive(true);
                    _baseItemBuilder.Render(baseItem);
                });
                _baseItemBtns.Add(clone.gameObject);
            }
        }

        private void Wipe()
        {
            foreach(var obj in _itemSlotBtns)
            {
                GameObject.Destroy(obj);
            }
            _itemSlotBtns.Clear();
        }

    }
}

