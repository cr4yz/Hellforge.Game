using UnityEngine;
using cakeslice;

namespace Hellforge.Game.Entities
{
    public class BaseEntity : MonoBehaviour
    {

        [SerializeField]
        protected bool selectable;
        private Outline _outlineComponent;

        void Awake()
        {
            _Awake();
        }

        void Start()
        {
            _outlineComponent = GetComponentInChildren<Outline>();
            if(_outlineComponent != null)
            {
                _outlineComponent.enabled = false;
            }

            _Start();
        }

        void Update()
        {
            _Update();
        }

        protected void SetOutlineColor(int color)
        {
            if (_outlineComponent != null)
            {
                _outlineComponent.color = color;
            }
        }

        void OnMouseEnter()
        {
            if (!selectable)
            {
                return;
            }

            if (_outlineComponent != null)
            {
                _outlineComponent.enabled = true;
            }
        }

        void OnMouseExit()
        {
            if (!selectable)
            {
                return;
            }

            if (_outlineComponent != null)
            {
                _outlineComponent.enabled = false;
            }
        }

        protected virtual void _Start() { }
        protected virtual void _Awake() { }
        protected virtual void _Update() { }

    }
}
