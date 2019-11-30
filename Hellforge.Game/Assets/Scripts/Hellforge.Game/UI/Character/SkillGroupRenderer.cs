using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Hellforge.Game.UI
{
    public class SkillGroupRenderer : MonoBehaviour
    {

        [SerializeField]
        private Text _groupNametext;
        [SerializeField]
        private SkillIconRenderer _skillIconTemplate;

        private void Awake()
        {
            _skillIconTemplate.gameObject.SetActive(false);
        }

        public void Render(string groupName, List<SkillEntry> skills)
        {
            _groupNametext.text = groupName;

            foreach(var skill in skills)
            {
                var clone = GameObject.Instantiate(_skillIconTemplate, _skillIconTemplate.transform.parent);
                clone.Render(skill);
                clone.gameObject.SetActive(true);
            }
        }

    }
}

