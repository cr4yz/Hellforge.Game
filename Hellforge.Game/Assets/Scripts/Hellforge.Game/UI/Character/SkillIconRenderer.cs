using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Hellforge.Game.World;
using Hellforge.Core.Entities;

namespace Hellforge.Game.UI
{
    public class SkillIconRenderer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {

        [SerializeField]
        private Text _rankText; 
        [SerializeField]
        private SkillLabelRenderer _skillLabel;
        private string _skillName;

        private bool _hasSkillLabel;

        private void OnDisable()
        {
            if(_hasSkillLabel)
            {
                _skillLabel.gameObject.SetActive(false);
            }
        }

        public void Render(SkillEntry skill)
        {
            var currentRank = GameWorld.Instance.Character.Allocations.GetPoints(AllocationType.Skill, skill.Name);
            _rankText.text = $"{currentRank}/{skill.MaxRank}";
            _skillName = skill.Name;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _hasSkillLabel = true;
            UpdateSkillLabel();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _hasSkillLabel = false;
            _skillLabel.gameObject.SetActive(false);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            var skillData = D4Data.Instance.Hellforge.GameData.Skills.First(x => x.Name == _skillName);
            var currentRank = GameWorld.Instance.Character.Allocations.GetPoints(AllocationType.Skill, _skillName);
            var increment = 1;

            if(eventData.button == PointerEventData.InputButton.Right)
            {
                if(currentRank == 0)
                {
                    return;
                }
                increment = - 1;
            }
            else
            {
                if(currentRank == skillData.MaxRank)
                {
                    return;
                }
            }

            GameWorld.Instance.Character.Allocations.IncrementAllocation(AllocationType.Skill, _skillName, increment);

            UpdateSkillLabel();
            Render(skillData);
        }

        private void UpdateSkillLabel()
        {
            var skillData = D4Data.Instance.Hellforge.GameData.Skills.First(x => x.Name == _skillName);
            var rt = _skillLabel.GetComponent<RectTransform>();
            var oldParent = _skillLabel.transform.parent;
            _skillLabel.transform.SetParent(transform);
            rt.anchoredPosition = Vector3.zero;
            _skillLabel.transform.SetParent(oldParent, true);
            _skillLabel.gameObject.SetActive(true);
            _skillLabel.Render(skillData);
        }

    }
}

