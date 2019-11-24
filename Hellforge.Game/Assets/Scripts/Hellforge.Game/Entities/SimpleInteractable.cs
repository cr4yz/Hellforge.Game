using UnityEngine;
using cakeslice;

namespace Hellforge.Game.Entities
{
    public class SimpleInteractable : MonoBehaviour, IInteractable
    {

        private Outline _outlineComponent;

        void Start()
        {
            _outlineComponent = GetComponentInChildren<Outline>();
            _outlineComponent.enabled = false;
        }

        void OnMouseOver()
        {
            _outlineComponent.enabled = true;
        }

        void OnMouseExit()
        {
            _outlineComponent.enabled = false;
        }
    }
}
