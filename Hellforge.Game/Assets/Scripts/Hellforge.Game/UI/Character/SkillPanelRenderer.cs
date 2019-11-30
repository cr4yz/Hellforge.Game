using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Hellforge.Game.World;

namespace Hellforge.Game.UI
{
    public class SkillPanelRenderer : MonoBehaviour
    {

        [SerializeField]
        private SkillGroupRenderer _skillGroupTemplate;

        private void Awake()
        {
            _skillGroupTemplate.gameObject.SetActive(false);

            if (GameWorld.Instance.Character != null)
            {
                Render(GameWorld.Instance.Character.Class);
            }
        }

        public void Render(string className)
        {
            var skillGroups = D4Data.Instance.Hellforge.GameData.Skills.Where(x => x.Class == className).GroupBy(x => x.Group);

            foreach (var group in skillGroups)
            {
                var groupName = group.Key;
                var clone = GameObject.Instantiate(_skillGroupTemplate, _skillGroupTemplate.transform.parent);
                clone.Render(groupName, group.ToList());
                clone.gameObject.SetActive(true);
                clone.transform.SetSiblingIndex(GetGroupIndex(groupName));
            }
        }

        private int GetGroupIndex(string groupName)
        {
            switch(groupName)
            {
                case "Basic":
                    return 0;
                case "Fury":
                    return 1;
                case "Defensive":
                    return 2;
                case "Brawling":
                    return 3;
                case "WeaponMastery":
                    return 4;
                case "Ultimate":
                    return 5;
            }
            return 0;
        }

    }
}

