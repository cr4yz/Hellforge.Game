using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hellforge.Game.UI
{
    public class AffixEditor : MonoBehaviour
    {

        [SerializeField]
        private Button _itemSlotBtnTemplate;
        [SerializeField]
        private Button _affixBtnTemplate;
        [SerializeField]
        private Button _createAffixButton;
        [SerializeField]
        private AffixBuilder _affixBuilder;
        [SerializeField]
        private Button _compileButton;
        [SerializeField]
        private Button _reloadButton;

        private List<GameObject> _itemSlotBtns = new List<GameObject>();
        private List<GameObject> _affixBtns = new List<GameObject>();
        private string _selectedItemSlot;

        void Awake()
        {
            _itemSlotBtnTemplate.gameObject.SetActive(false);
            _affixBtnTemplate.gameObject.SetActive(false);
            _affixBuilder.gameObject.SetActive(false);
            _createAffixButton.gameObject.SetActive(false);

            if (D4Data.Instance == null)
            {
                gameObject.AddComponent<D4Data>();
            }

            _createAffixButton.onClick.AddListener(delegate ()
            {
                var affix = new AffixEntry();
                affix.ItemSlot = _selectedItemSlot;
                AddAffix(affix);
                _affixBuilder.gameObject.SetActive(true);
                _affixBuilder.Render(affix);
            });

            _compileButton.onClick.AddListener(delegate ()
            {
                var filePath = D4Data.Instance.Hellforge.CompileToSingleFile();
#if UNITY_EDITOR
                var fileName = System.IO.Path.GetFileName(filePath);
                UnityEditor.FileUtil.CopyFileOrDirectory(filePath, Application.dataPath + "\\" + fileName);
                UnityEditor.AssetDatabase.Refresh();
#endif
                D4Data.Instance.Hellforge.ReloadData();
            });

            _reloadButton.onClick.AddListener(delegate ()
            {
                D4Data.Instance.Hellforge.ReloadData();
                Render();
            });
        }

        void Start()
        {
            Render();
        }

        public void AddAffix(AffixEntry affix)
        {
            var affixList = D4Data.Instance.Hellforge.GameData.Affixes.ToList();
            affixList.Add(affix);
            D4Data.Instance.Hellforge.GameData.Affixes = affixList.ToArray();
            Render();
        }

        public void RemoveAffix(AffixEntry entry)
        {
            var affixList = D4Data.Instance.Hellforge.GameData.Affixes.ToList();
            affixList.Remove(entry);
            D4Data.Instance.Hellforge.GameData.Affixes = affixList.ToArray();
            Render();
        }

        public void Render()
        {
            RenderItemSlots();
            if(_selectedItemSlot != null)
            {
                RenderAffixes(_selectedItemSlot);
            }
        }

        private void RenderItemSlots()
        {
            foreach(var obj in _itemSlotBtns)
            {
                GameObject.Destroy(obj);
            }
            _itemSlotBtns.Clear();

            foreach(var slot in D4Data.Instance.Hellforge.GameData.ItemSlots)
            {
                var clone = GameObject.Instantiate(_itemSlotBtnTemplate, _itemSlotBtnTemplate.transform.parent);
                clone.GetComponentInChildren<Text>().text = slot;
                clone.onClick.AddListener(delegate ()
                {
                    _affixBuilder.gameObject.SetActive(false);
                    RenderAffixes(slot);
                });
                clone.gameObject.SetActive(true);
                _itemSlotBtns.Add(clone.gameObject);
            }
        }

        private void RenderAffixes(string slotName)
        {
            _selectedItemSlot = slotName;
            _createAffixButton.gameObject.SetActive(true);

            foreach (var obj in _affixBtns)
            {
                GameObject.Destroy(obj);
            }
            _affixBtns.Clear();

            var affixes = D4Data.Instance.Hellforge.GameData.Affixes.Where(x => x.ItemSlot == slotName);
            foreach(var affix in affixes)
            {
                var clone = GameObject.Instantiate(_affixBtnTemplate, _affixBtnTemplate.transform.parent);
                clone.GetComponentInChildren<Text>().text = affix.Name;
                clone.onClick.AddListener(delegate ()
                {
                    _affixBuilder.gameObject.SetActive(true);
                    _affixBuilder.Render(affix);
                });
                clone.gameObject.SetActive(true);
                _affixBtns.Add(clone.gameObject);
            }
        }

    }
}

