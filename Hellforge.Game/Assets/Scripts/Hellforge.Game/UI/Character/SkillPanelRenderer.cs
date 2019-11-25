using System.Linq;
using UnityEngine;
using Hellforge.Game.World;

namespace Hellforge.Game.UI
{
    public class SkillPanelRenderer : MonoBehaviour
    {

        [SerializeField]
        private SkillGroupRenderer _skillGroupTemplate;

        private void Start()
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
            }
        }

    }
}

