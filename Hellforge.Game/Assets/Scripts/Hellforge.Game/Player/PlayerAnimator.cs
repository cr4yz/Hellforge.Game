using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hellforge.Game.Player
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerAnimator : MonoBehaviour
    {

        private PlayerController _controller;
        [SerializeField]
        private Animator _animator;
        [SerializeField]
        private Transform _body;

        private void Start()
        {
            _controller = GetComponent<PlayerController>();
        }

        private void Update()
        {
            _animator.SetBool("IsMoving", _controller.IsMoving);
            if (_controller.IsMoving && _controller.Direction != Vector3.zero)
            {
                _body.rotation = Quaternion.Slerp(
                    _body.rotation,
                    Quaternion.LookRotation(_controller.Direction),
                    Time.deltaTime * 8f
                );
            }
        }

    }
}

