using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hellforge.Game.Entities;

namespace Hellforge.Game.UI
{
    public class SkillDPSRenderer : MonoBehaviour
    {

        [SerializeField]
        private Image _skillIcon;
        [SerializeField]
        private Text _skillNameText;
        [SerializeField]
        private Text _skillRankText;
        [SerializeField]
        private GameObject _skillDamageTemplate;
        [SerializeField]
        private Text _skillDamageNameText;
        [SerializeField]
        private Text _skillDamageAmountText;

        private List<GameObject> _skillDamageTemplates = new List<GameObject>();

        void Awake()
        {
            _skillDamageTemplate.gameObject.SetActive(false);
        }

        public void Render(SkillEntry skill, int rank, DamageInfo dmgInfo)
        {
            Wipe();

            foreach(var dmg in dmgInfo.Damages)
            {
                _skillDamageNameText.text = dmg.DamageType.ToString();
                _skillDamageAmountText.text = $"{dmg.Min} - {dmg.Max}";
                var clone = GameObject.Instantiate(_skillDamageTemplate, _skillDamageTemplate.transform.parent);
                clone.gameObject.SetActive(true);
                _skillDamageTemplates.Add(clone);
            }

            _skillNameText.text = skill.Name;
            _skillRankText.text = $"ARCHETYPE: {skill.Archetype}, RANK {rank}/{skill.MaxRank}";
        }

        private void Wipe()
        {
            foreach(var obj in _skillDamageTemplates)
            {
                GameObject.Destroy(obj);
            }

            _skillDamageTemplates.Clear();
        }

    }
}


