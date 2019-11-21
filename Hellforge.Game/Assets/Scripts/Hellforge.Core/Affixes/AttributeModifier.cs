using Hellforge.Core.Entities;

namespace Hellforge.Core.Affixes
{
    public class AttributeModifier : AffixNode
    {
        private readonly string _attribute;
        private readonly int _amount;

        public AttributeModifier(Affix affix)
            : base(affix)
        {
            var amount = Affix.AffixData.Data[Affix.TierDataIndex].Amount;

            if(amount == -1)
            {
                var minRoll = Affix.AffixData.Data[Affix.TierDataIndex].Minimum;
                var maxRoll = Affix.AffixData.Data[Affix.TierDataIndex].Maximum;
                amount = Roll(minRoll, maxRoll, Affix.Roll);
            }

            _amount = amount;
            _attribute = Affix.AffixData.Attribute;
        }

        public override void Enable()
        {
            Add(Affix.Character);
        }

        public override void Disable()
        {
            Subtract(Affix.Character);
        }

        private void Add(Character character)
        {
            Affix.Character.Attributes[_attribute] += _amount;
        }

        private void Subtract(Character character)
        {
            Affix.Character.Attributes[_attribute] -= _amount;
        }

        public static int Roll(int a, int b, int roll)
        {
            return (int)(((b - a) * (roll / 100f)) + a);
        }
    }
}
