using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hellforge.Game.Entities;

namespace Hellforge.Game.UI
{
    public class EntityOverheadPanel : MonoBehaviour
    {

        [SerializeField]
        private Text _entityNameText;
        [SerializeField]
        private Image _healthBarImage;

        private BaseEntity _entity;

        void Start()
        {
            _entity = gameObject.GetComponentInParent<BaseEntity>();
            _entityNameText.text = _entity.DisplayName;
        }

        void Update()
        {
            if(_entity is IDamageable idmg)
            {
                _healthBarImage.fillAmount = (float)idmg.Health / idmg.MaxHealth;
            }
        }

    }
}

