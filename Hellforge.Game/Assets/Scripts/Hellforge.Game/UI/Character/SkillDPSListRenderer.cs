using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Hellforge.Core.Entities;
using Hellforge.Game.Entities;
using Hellforge.Game.World;

namespace Hellforge.Game.UI
{
    public class SkillDPSListRenderer : MonoBehaviour
    {

        [SerializeField]
        private SkillDPSRenderer _skillDpsTemplate;

        private List<SkillDPSRenderer> _skillDpsRenderers = new List<SkillDPSRenderer>();

        void Awake()
        {
            GameWorld.Instance.Character.OnChanged += () =>
            {
                Render(GameWorld.Instance.Hero);
            };
        }

        void OnEnable()
        {
            _skillDpsTemplate.gameObject.SetActive(false);
            if(GameWorld.Instance.Hero != null)
            {
                Render(GameWorld.Instance.Hero);
            }
        }

        private void Render(D4Hero hero)
        {
            Wipe();

            foreach(var skill in hero.Controller.SlottedSkills)
            {
                var skillData = D4Data.Instance.Hellforge.GameData.Skills.FirstOrDefault(x => x.Name == skill.Value.SkillName);
                if(skillData == null)
                {
                    Debug.Log("No skill data: " + skill.Value.SkillName);
                    continue;
                }
                var skillRank = hero.Character.Allocations.GetPoints(AllocationType.Skill, skill.Value.SkillName);
                var clone = GameObject.Instantiate(_skillDpsTemplate, _skillDpsTemplate.transform.parent);
                var dmgInfo = skill.Value.BuildDamageInfo(true);
                hero.PostProcessDamage(dmgInfo);
                clone.Render(skillData, skillRank, dmgInfo);
                clone.gameObject.SetActive(true);
                _skillDpsRenderers.Add(clone);
            }

            gameObject.RebuildLayout();
        }

        private void Wipe()
        {
            foreach(var obj in _skillDpsRenderers)
            {
                GameObject.Destroy(obj.gameObject);
            }
            _skillDpsRenderers.Clear();
        }

    }
}