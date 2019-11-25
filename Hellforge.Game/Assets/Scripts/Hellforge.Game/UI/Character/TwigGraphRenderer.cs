using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Hellforge.Core.Entities;
using Hellforge.Core.Twig;
using Hellforge.Game.World;

namespace Hellforge.Game.UI
{
    public class TwigGraphRenderer : MonoBehaviour
    {

        [SerializeField]
        private RectTransform _renderPanel;
        [SerializeField]
        private TwigNodeRenderer _twigNodeTemplate;

        private bool _rendered;

        private List<TwigNodeRenderer> _nodeRenderers = new List<TwigNodeRenderer>();

        private void Start()
        {
            if (GameWorld.Instance.Character != null)
            {
                var classTree = D4Data.Instance.Hellforge.GameData.TalentTrees.First(x => x.Class == GameWorld.Instance.Character.Class);
                Render(classTree, GameWorld.Instance.Character);
            }
        }

        public void Render(TwigGraph graph, Character character = null)
        {
            Wipe();

            var scaleModifier = new Vector2(_renderPanel.sizeDelta.x / graph.Width, _renderPanel.sizeDelta.y / graph.Height);

            foreach (var node in graph.Nodes)
            {
                if(node.Type == "Route")
                {
                    continue;
                }
                var clone = GameObject.Instantiate(_twigNodeTemplate, _twigNodeTemplate.transform.parent);
                clone.gameObject.SetActive(true);
                clone.Render(node, character);
                clone.GetComponent<RectTransform>().anchoredPosition *= scaleModifier;
                _nodeRenderers.Add(clone);
            }
        }

        public void Wipe()
        {
            _twigNodeTemplate.gameObject.SetActive(false);

            foreach (var node in _nodeRenderers)
            {
                GameObject.Destroy(node.gameObject);
            }

            _nodeRenderers.Clear();
        }

    }
}

