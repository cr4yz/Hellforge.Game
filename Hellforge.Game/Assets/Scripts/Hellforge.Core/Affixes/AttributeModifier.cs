using System;

namespace Hellforge.Core.Affixes
{
    public class AttributeModifier : AffixNode
    {
        private readonly AttributeName _attribute;
        private readonly int _amount;

        public AttributeModifier(Affix affix)
            : base(affix)
        {
            var amount = Affix.AffixData.Data[Affix.TierDataIndex].Amount;

            if(amount == 0)
            {
                var minRoll = Affix.AffixData.Data[Affix.TierDataIndex].Minimum;
                var maxRoll = Affix.AffixData.Data[Affix.TierDataIndex].Maximum;
                amount = Roll(minRoll, maxRoll, Affix.Roll);
            }

            _amount = amount;
            Enum.TryParse(Affix.AffixData.Attribute, out _attribute);
        }

        public override void Enable()
        {
            Affix.Character.Attributes[_attribute] += _amount;
        }

        public override void Disable()
        {
            Affix.Character.Attributes[_attribute] -= _amount;
        }

        public static int Roll(int a, int b, int roll)
        {
            return (int)(((b - a) * (roll / 100f)) + a);
        }
    }
}
