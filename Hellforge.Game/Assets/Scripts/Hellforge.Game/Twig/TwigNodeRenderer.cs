using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using UnityEngine.UI;
using Hellforge.Core.Entities;
using Hellforge.Core.Twig;

namespace Hellforge.Game.Twig
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

            var currentTier = character?.GetAffixTier(Node.Affix) ?? 0;

            SetTier(currentTier);
        }

        public void SetTier(int tier)
        {
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

            var tier = _character?.GetAffixTier(Node.Affix) ?? 0;
            var prevParnet = _nodeLabel.parent;
            _nodeLabel.SetParent(transform, false);
            _nodeLabel.localScale = Vector3.one;
            _nodeLabel.anchoredPosition = Vector3.zero;
            _nodeLabelTitle.text = affix.Name;
            _nodeLabelDescription.text = affix.ParseDescription(Mathf.Max(tier - 1, 0), 100);
            SetTier(tier);
            _nodeLabel.SetParent(prevParnet, true);
            Invoke("RebuildNodeLabelLayout", 0.1f);
            _nodeLabel.gameObject.SetActive(true);
        }

        public void RebuildNodeLabelLayout()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(_nodeLabel);
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
                    affix = _character.AddAffix(Node.Affix, 1, 100);
                    SetTier(1);
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

                newTier = Mathf.Clamp(newTier, 0, affix.TierCount);

                if(newTier == affix.Tier)
                {
                    return;
                }
                else if(newTier == 0)
                {
                    _character.RemoveAffix(Node.Affix);
                    _character.Allocations.RemoveAllocation(Node.Affix);
                    SetTier(0);
                }
                else
                {
                    var newAffix = _character.UpdateAffix(Node.Affix, newTier);
                    SetTier(newTier);
                    _character.Allocations.SetAllocation(AllocationType.Talent, Node.Affix, newTier);
                }

                OnPointerEnter();
            }
        }

    }
}

