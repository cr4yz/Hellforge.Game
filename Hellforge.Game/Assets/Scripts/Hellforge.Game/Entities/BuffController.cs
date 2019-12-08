using System.Collections.Generic;
using UnityEngine;

namespace Hellforge.Game.Entities
{
    [RequireComponent(typeof(BaseEntity))]
    public class BuffController : MonoBehaviour
    {

        private List<BaseBuff> _autoBuffs = new List<BaseBuff>();
        private List<BaseBuff> _manualBuffs = new List<BaseBuff>();

        private void Update()
        {
            for(int i = _autoBuffs.Count - 1; i >= 0; i--)
            {
                _autoBuffs[i].Update();
                if (_autoBuffs[i].State == BaseBuff.BuffState.Expired)
                {
                    _autoBuffs.RemoveAt(i);
                }
            }

            for(int i = 0; i < _manualBuffs.Count; i++)
            {
                _manualBuffs[i].Update();
            }
        }

        public void AddDamageOverTime(float duration, DamageInfo dmgInfo)
        {
            var buff = new DamageOverTime(GetComponent<BaseEntity>(), duration, dmgInfo);
            buff.Start();
            _autoBuffs.Add(buff);
        }

        public void AddBuff(BaseBuff buff)
        {
            if(_manualBuffs.Contains(buff))
            {
                return;
            }
            _manualBuffs.Add(buff);
        }

    }
}

