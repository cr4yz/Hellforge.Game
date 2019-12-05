using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hellforge.Game.UI
{
    public class AttributeEditor : MonoBehaviour
    {

        [SerializeField]
        private AttributeBuilder _builder;
        [SerializeField]
        private Button _attrBtnTemplate;
        [SerializeField]
        private Button _createAttributeButton;

        private List<GameObject> _clones = new List<GameObject>();
        
        private void Start()
        {
            _attrBtnTemplate.gameObject.SetActive(false);
            _builder.gameObject.SetActive(false);
            _createAttributeButton.onClick.AddListener(delegate ()
            {
                var attr = new AttributeEntry()
                {
                    Name = "NewAttribute",
                    Category = "Offense"
                };
                AddAttribute(attr);
                _builder.gameObject.SetActive(true);
                _builder.Render(attr);
            });

            Render();
        }

        public void Render()
        {
            Wipe();

            foreach(var attr in D4Data.Instance.Hellforge.GameData.Attributes)
            {
                var clone = GameObject.Instantiate(_attrBtnTemplate, _attrBtnTemplate.transform.parent);
                clone.gameObject.SetActive(true);
                clone.GetComponentInChildren<Text>().text = attr.Name;
                clone.onClick.AddListener(delegate ()
                {
                    _builder.gameObject.SetActive(true);
                    _builder.Render(attr);
                });
                _clones.Add(clone.gameObject);
            }
        }

        private void Wipe()
        {
            foreach(var obj in _clones)
            {
                GameObject.Destroy(obj);
            }

            _clones.Clear();
        }

        public void RemoveAttribute(AttributeEntry attr)
        {
            var attrList = D4Data.Instance.Hellforge.GameData.Attributes.ToList();
            attrList.Remove(attr);
            D4Data.Instance.Hellforge.GameData.Attributes = attrList.ToArray();
            Render();
        }

        public void AddAttribute(AttributeEntry attr)
        {
            var attrList = D4Data.Instance.Hellforge.GameData.Attributes.ToList();
            attrList.Add(attr);
            D4Data.Instance.Hellforge.GameData.Attributes = attrList.ToArray();
            Render();
        }

    }
}