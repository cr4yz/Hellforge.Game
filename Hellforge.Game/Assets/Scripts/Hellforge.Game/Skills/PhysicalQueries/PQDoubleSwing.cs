using System;
using System.Collections.Generic;
using UnityEngine;
using Hellforge.Game.Entities;

namespace Hellforge.Game.Skills
{
    public class PQDoubleSwing : PhysicalQuery
    {
        [SerializeField]
        private Transform _leftHand;
        [SerializeField]
        private Transform _rightHand;
        [SerializeField]
        private float _rotationDegrees = 90f;
        [SerializeField]
        private float _secondsToComplete = 0.75f;

        private bool _rotating;
        private float _degreesRotated;

        private void OnCollisionEnter(Collision other)
        {
            var ent = other.gameObject.GetComponent<BaseEntity>();
            if(ent != null)
            {
                HitDetected(ent);
            }
        }

        private void Update()
        {
            if(!_rotating)
            {
                return;
            }

            var rotationSpeed = (_rotationDegrees / _secondsToComplete) * Time.deltaTime;
            _degreesRotated += rotationSpeed;

            _leftHand.RotateAround(_leftHand.position, Vector3.up, rotationSpeed);
            _rightHand.RotateAround(_rightHand.position, Vector3.up, -rotationSpeed);

            if(_degreesRotated > _rotationDegrees)
            {
                _rotating = false;
                Complete();
            }
        }

        public void Begin(Vector3 position, Quaternion startRotation)
        {
            if(_rotating)
            {
                throw new Exception("Don't re-use PhysicalQueries");
            }

            transform.rotation = startRotation;
            transform.position = position;
            _rotating = true;
        }
    }
}

