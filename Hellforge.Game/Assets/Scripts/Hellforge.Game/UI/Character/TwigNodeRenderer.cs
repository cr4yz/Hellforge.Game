using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEngine.UI;
using Hellforge.Core.Entities;
using Hellforge.Core.Twig;

namespace Hellforge.Game.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class TwigNodeRenderer : MonoBehaviour
    {

        private Character _character;
        private RectTransform _rt;
        [SerializeField]
        private RectTransform _nodeLabel;
        [SerializeField]
        private Text _nodeLabelTitle;
        [SerializeField]
        private Text _nodeLabelRank;
        [SerializeField]
        private Text _nodeTierText;
        [SerializeField]
        private Text _nodeLabelDescription;

        public TwigNode Node { get; private set; }

        public void Render(TwigNode node, Character character)
        {
            _nodeLabel.gameObject.SetActive(false);
            _rt = GetComponent<RectTransform>();
            _rt.anchoredPosition = new Vector2(node.X, -node.Y);
            _character = character;
            Node = node;

            SetTier(character.GetAffixTier(Node.Affix));
        }

        public void SetTier(int tier)
        {
            tier++;

            var maxTiers = Node.Graph.Hellforge.GetAffixTierCount(Node.Affix);
            var str = $"{tier}/{maxTiers}";
            _nodeLabelRank.text = str;
            if (tier == 0)
            {
                _nodeTierText.transform.parent.gameObject.SetActive(false);
                return;
            }
            _nodeTierText.transform.parent.gameObject.SetActive(true);
            _nodeTierText.gameObject.SetActive(tier > 0);
            _nodeTierText.text = str;
        }

        public void OnPointerEnter()
        {
            var affix = Node.Graph.Hellforge.GameData.Affixes.FirstOrDefault(x => x.Name == Node.Affix);

            if(affix == null)
            {
                return;
            }

            var tier = _character.GetAffixTier(Node.Affix);
            var prevParnet = _nodeLabel.parent;
            _nodeLabel.SetParent(transform, false);
            _nodeLabel.localScale = Vector3.one;
            _nodeLabel.anchoredPosition = Vector3.zero;
            _nodeLabelTitle.text = affix.Name;
            _nodeLabelDescription.text = affix.ParseDescription(Mathf.Max(tier, 0), 100);
            SetTier(tier);
            _nodeLabel.SetParent(prevParnet, true);
            _nodeLabel.gameObject.SetActive(true);
            _nodeLabel.gameObject.RebuildLayout();
        }

        public void OnPointerExit()
        {
            _nodeLabel.gameObject.SetActive(false);
        }

        public void OnPointerDown(BaseEventData data)
        {
            var pointerData = data as PointerEventData;
            var affix = _character.GetAffix(Node.Affix);

            if(affix == null)
            {
                if (pointerData.button == PointerEventData.InputButton.Left)
                {
                    affix = _character.AddAffix(Node.Affix, 0, 100);
                    SetTier(0);
                }
            }
            else 
            {
                var newTier = affix.Tier;

                if (pointerData.button == PointerEventData.InputButton.Left)
                {
                    newTier++;
                }
                else if (pointerData.button == PointerEventData.InputButton.Right)
                {
                    newTier--;
                }

                newTier = Mathf.Clamp(newTier, -1, affix.TierCount - 1);

                if (newTier == affix.Tier)
                {
                    return;
                }
                else if(newTier == -1)
                {
                    _character.RemoveAffix(Node.Affix);
                    _character.Allocations.RemoveAllocation(Node.Affix);
                }
                else
                {
                    _character.UpdateAffix(Node.Affix, newTier);
                    _character.Allocations.SetAllocation(AllocationType.Talent, Node.Affix, newTier + 1);
                }

                SetTier(newTier);

                OnPointerEnter();
            }
        }

    }
}

