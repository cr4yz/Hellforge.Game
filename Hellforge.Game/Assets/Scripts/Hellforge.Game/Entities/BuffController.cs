using System.Collections.Generic;
using UnityEngine;

namespace Hellforge.Game.Entities
{
    [RequireComponent(typeof(BaseEntity))]
    public class BuffController : MonoBehaviour
    {

        private List<BaseBuff> _buffs = new List<BaseBuff>();

        private void Update()
        {
            for(int i = _buffs.Count - 1; i >= 0; i--)
            {
                _buffs[i].Update();
                if (_buffs[i].Expired)
                {
                    _buffs.RemoveAt(i);
                }
            }
        }

        public void AddDamageOverTime(float duration, DamageInfo dmgInfo)
        {
            _buffs.Add(new DamageOverTime(GetComponent<BaseEntity>(), duration, dmgInfo));
        }

    }
}

