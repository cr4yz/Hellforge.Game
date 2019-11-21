using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using Hellforge.Game.World;

namespace Hellforge.Game.Player
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class PlayerController : MonoBehaviour
    {

        public float BaseMoveSpeed = 2.5f;
        private NavMeshAgent _agent;
        private NavMeshPath _path;
        private Vector3[] _waypoints = new Vector3[256];
        private int _waypointCount;
        private int _currentWaypoint;
        [SerializeField]
        private Camera _playerCam;

        public bool IsMoving { get; private set; }
        public Vector3 Direction { get; private set; }
        public Vector3 Velocity { get; private set; }

        private void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            _path = new NavMeshPath();
        }

        private void Update()
        {
            TryUpdatePath();
            MoveAlongPath();
        }

        private void MoveAlongPath()
        {
            if(IsMoving)
            {
                if(Vector3.Distance(transform.position, _waypoints[_currentWaypoint]) <= 0.1f)
                {
                    _currentWaypoint++;
                    if(_currentWaypoint == _waypointCount)
                    {
                        IsMoving = false;
                        return;
                    }
                }

                var speed = BaseMoveSpeed;
                var incSpeed = GameWorld.Instance.Character.GetAttribute("MovementSpeed");

                if(incSpeed != 0)
                {
                    speed += incSpeed / 100f * BaseMoveSpeed;
                }

                Direction = (_waypoints[_currentWaypoint] - transform.position).normalized;
                Velocity = Direction * speed;
                transform.Translate(Velocity * Time.deltaTime);
            }
        }

        private void TryUpdatePath()
        {
            if (Input.GetMouseButton(0)
                && !EventSystem.current.IsPointerOverGameObject())
            {
                if (Physics.Raycast(_playerCam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100))
                {
                    if (_agent.CalculatePath(hit.point, _path))
                    {
                        IsMoving = true;
                        _currentWaypoint = 0;
                        _waypointCount = _path.corners.Length;
                        for (int i = 0; i < _path.corners.Length; i++)
                        {
                            _waypoints[i] = _path.corners[i];
                        }
                    }
                }
            }
        }

    }
}
