using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hellforge.Game.World;

namespace Hellforge.Game.UI
{
    public class SkillLabelRenderer : MonoBehaviour
    {

        [SerializeField]
        private Text _nameText;
        [SerializeField]
        private Text _archetypeText;
        [SerializeField]
        private Text _rankText;
        [SerializeField]
        private Text _descriptionText;
        [SerializeField]
        private Text _nextRankText;

        public void Render(SkillEntry skill)
        {
            var rank = GameWorld.Instance.Character.Allocations.GetPoints(Core.Entities.AllocationType.Skill, skill.Name);
            _nameText.text = skill.Name;
            if(skill.Archetype != null)
            {
                _archetypeText.gameObject.SetActive(true);
                _archetypeText.text = skill.Archetype;
            }
            else
            {
                _archetypeText.gameObject.SetActive(false);
            }
            _rankText.text = "RANK " + rank;
            _descriptionText.text = skill.ParseDescription(rank);
            _nextRankText.text = skill.ParseNextRankText(rank + 1);

            gameObject.RebuildLayout();
        }

    }
}

