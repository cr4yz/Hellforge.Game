using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Hellforge.Game.Skills;
using Hellforge.Game.World;

namespace Hellforge.Game.UI
{
    public class SkillSlotRenderer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {

        [SerializeField]
        private Text _skillNameText;
        [SerializeField]
        private SkillSlot _skillSlot;
        [SerializeField]
        private SkillSelectionRenderer _skillSelection;

        private string _metaIdentifier;

        private void Start()
        {
            _skillNameText.gameObject.SetActive(false);
            _skillSelection.gameObject.SetActive(false);
            _metaIdentifier = Player.PlayerController.SkillMetaPrefix + _skillSlot.ToString();

            if (GameWorld.Instance.Character.Meta.ContainsKey(_metaIdentifier))
            {
                Render(GameWorld.Instance.Character.Meta[_metaIdentifier]);
            }
        }

        public void Render(string skillName)
        {
            _skillNameText.gameObject.SetActive(true);
            _skillNameText.text = skillName;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _skillSelection.gameObject.SetActive(true);
            _skillSelection.GetComponent<RectTransform>().PositionTo(GetComponent<RectTransform>());
            _skillSelection.Render();
            _skillSelection.OnSkillChosen = delegate (string skillName)
            {
                if(skillName == null)
                {
                    Render(string.Empty);
                    GameWorld.Instance.Hero.Controller.UnslotSkill(_skillSlot);
                    GameWorld.Instance.Character.Meta.Remove(_metaIdentifier);
                    return;
                }
                Render(skillName);
                GameWorld.Instance.Hero.Controller.SlotSkill(_skillSlot, skillName);
                GameWorld.Instance.Character.Meta[_metaIdentifier] = skillName;
            };
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            //var skillLabel = FindObjectOfType<SkillLabelRenderer>();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
        }
    }
}
