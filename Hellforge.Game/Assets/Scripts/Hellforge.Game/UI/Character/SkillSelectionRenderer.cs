using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hellforge.Game.World;
using Hellforge.Core.Entities;

namespace Hellforge.Game.UI
{
    public class SkillSelectionRenderer : MonoBehaviour
    {

        public Action<string> OnSkillChosen;

        [SerializeField]
        private Button _noneButton;
        [SerializeField]
        private Button _skillBtnTemplate;
        private List<GameObject> _clones = new List<GameObject>();

        private void Start()
        {
            _skillBtnTemplate.gameObject.SetActive(false);
        }

        public void Render()
        {
            Wipe();

            _noneButton.onClick.AddListener(delegate ()
            {
                OnSkillChosen?.Invoke(null);
                gameObject.SetActive(false);
            });

            var allocatedSkills = GameWorld.Instance.Character.Allocations.Points.FindAll(x => x.Type == AllocationType.Skill);
            foreach(var skill in allocatedSkills)
            {
                var clone = GameObject.Instantiate(_skillBtnTemplate, _skillBtnTemplate.transform.parent);
                clone.gameObject.SetActive(true);
                clone.GetComponentInChildren<Text>().text = skill.Identifier;
                clone.onClick.AddListener(delegate ()
                {
                    OnSkillChosen?.Invoke(skill.Identifier);
                    gameObject.SetActive(false);
                });
                _clones.Add(clone.gameObject);
            }
        }

        public void Wipe()
        {
            _noneButton.onClick.RemoveAllListeners();

            foreach(var obj in _clones)
            {
                GameObject.Destroy(obj);
            }
            _clones.Clear();
        }

    }
}

