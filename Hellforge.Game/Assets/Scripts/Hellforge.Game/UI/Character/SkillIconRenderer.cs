using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Hellforge.Game.UI
{
    public class SkillIconRenderer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {

        [SerializeField]
        private SkillLabelRenderer _skillLabel;
        private string _skillName;
        private string _skillClass;

        public void Render(SkillEntry skill)
        {
            _skillName = skill.Name;
            _skillClass = skill.Class;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            var skillData = D4Data.Instance.Hellforge.GameData.Skills.First(x => x.Name == _skillName && x.Class == _skillClass);
            var rt = _skillLabel.GetComponent<RectTransform>();
            var oldParent = _skillLabel.transform.parent;
            _skillLabel.transform.SetParent(transform);
            rt.anchoredPosition = Vector3.zero;
            _skillLabel.transform.SetParent(oldParent, true);
            _skillLabel.gameObject.SetActive(true);
            _skillLabel.Render(skillData);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _skillLabel.gameObject.SetActive(false);
        }

    }
}

