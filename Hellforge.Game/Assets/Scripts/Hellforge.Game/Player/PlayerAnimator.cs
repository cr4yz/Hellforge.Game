using System.Linq;
using System.Collections;
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
            _animator.SetFloat("MovementSpeed", _controller.Velocity.magnitude / _controller.BaseMoveSpeed);

            if (_controller.IsMoving && _controller.Direction != Vector3.zero)
            {
                _body.rotation = Quaternion.Slerp(
                    _body.rotation,
                    Quaternion.LookRotation(_controller.Direction),
                    Time.deltaTime * 8f
                );
            }
        }

        public void PlayState(string stateName, float time, bool isAttack)
        {
            _animator.Play(stateName);
            _animTime = time;
            _isAttack = isAttack;

            StartCoroutine("SetAnimationTime");
        }

        private float _animTime;
        private bool _isAttack;

        private IEnumerator SetAnimationTime()
        {
            yield return 0;
            var clipInfos = _animator.GetCurrentAnimatorClipInfo(0);
            if(clipInfos.Length == 0)
            {
                yield break;
            }
            var param = _isAttack ? "AttackSpeed" : "MovementSpeed";
            var clipInfo = _animator.GetCurrentAnimatorClipInfo(0)[0];
            _animator.SetFloat(param, clipInfo.clip.length / _animTime);
        }

    }
}

