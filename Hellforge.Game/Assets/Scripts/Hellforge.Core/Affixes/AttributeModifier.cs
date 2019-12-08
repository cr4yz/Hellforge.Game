using System;

namespace Hellforge.Core.Affixes
{
    public class AttributeModifier : AffixNode
    {
        private readonly AttributeName _attribute;
        private int _cachedAmount;
        private bool _cache;

        public AttributeModifier(Affix affix, bool cache = true)
            : base(affix)
        {
            if(cache)
            {
                _cachedAmount = GetAmount();
            }

            Enum.TryParse(Affix.AffixData.Attribute, out _attribute);
        }

        public override void Enable()
        {
            Affix.Character.Attributes[_attribute] += _cache ? _cachedAmount : GetAmount();
        }

        public override void Disable()
        {
            Affix.Character.Attributes[_attribute] -= _cache ? _cachedAmount : GetAmount();
        }

        public int GetAmount()
        {
            var amount = Affix.AffixData.Data[Affix.TierDataIndex].Amount;

            if (amount == 0)
            {
                var minRoll = Affix.AffixData.Data[Affix.TierDataIndex].Minimum;
                var maxRoll = Affix.AffixData.Data[Affix.TierDataIndex].Maximum;
                amount = Roll(minRoll, maxRoll, Affix.Roll);
            }

            return amount;
        }

        public static int Roll(int a, int b, int roll)
        {
            return (int)(((b - a) * (roll / 100f)) + a);
        }
    }
}
