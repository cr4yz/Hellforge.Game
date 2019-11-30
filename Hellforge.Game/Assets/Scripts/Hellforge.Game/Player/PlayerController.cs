using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using Hellforge.Game.World;
using Hellforge.Game.Skills;
using Hellforge.Game.Entities;

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

        // todo: read these from file
        private Dictionary<KeyCode, SkillSlot> _skillKeyBinds = new Dictionary<KeyCode, SkillSlot>()
        {
            { KeyCode.Mouse0, SkillSlot.Primary },
            { KeyCode.Mouse1, SkillSlot.Secondary },
            { KeyCode.Alpha1, SkillSlot.One },
            { KeyCode.Alpha2, SkillSlot.Two },
            { KeyCode.Alpha3, SkillSlot.Three },
            { KeyCode.Alpha4, SkillSlot.Four },
        };

        public Dictionary<SkillSlot, BaseSkill> SlottedSkills { get; private set; } = new Dictionary<SkillSlot, BaseSkill>();

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
            MoveAlongPath();
            CheckSkillInput();
            UpdateSkills();
        }

        public void SlotSkill(SkillSlot slot, BaseSkill skill)
        {
            if (SlottedSkills.ContainsKey(slot))
            {
                SlottedSkills[slot] = skill;
            }
            else
            {
                SlottedSkills.Add(slot, skill);
            }
        }

        public void MoveToDestination(Vector3 destination)
        {
            if (_agent.CalculatePath(destination, _path))
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

        public void StopMoving()
        {
            IsMoving = false;
        }

        public bool IsInputBlocked()
        {
            if(EventSystem.current.IsPointerOverGameObject())
            {
                return true;
            }

            foreach(var kvp in SlottedSkills)
            {
                if(kvp.Value.IsBusy() && kvp.Value.BlocksInput)
                {
                    return true;
                }
            }

            return false;
        }

        public Vector3 GetMouseHitPoint()
        {
            if (Physics.Raycast(_playerCam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100))
            {
                return hit.point; 
            }
            // todo: if mouse doesn't hit anything then return the nearest walkable area
            return Vector3.zero;
        }

        public Vector3 GetMouseHitPoint(out BaseEntity targetEntity)
        {
            targetEntity = null;
            if (Physics.Raycast(_playerCam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100))
            {
                targetEntity = hit.collider.gameObject.GetComponent<BaseEntity>();
                if(targetEntity != null)
                {
                    return hit.collider.transform.position;
                }
                return hit.point;
            }
            // todo: if mouse doesn't hit anything then return the nearest walkable area
            return Vector3.zero;
        }

        private void CheckSkillInput()
        {
            if(IsInputBlocked())
            {
                return;
            }

            foreach(var kvp in _skillKeyBinds)
            {
                if (Input.GetKey(kvp.Key)
                    && SlottedSkills.TryGetValue(kvp.Value, out BaseSkill skill))
                {
                    var hitPoint = GetMouseHitPoint(out BaseEntity targetEntity);
                    skill.Cast(targetEntity, hitPoint);

                    foreach (var kvp2 in SlottedSkills)
                    {
                        if(kvp2.Value != skill)
                            kvp2.Value.Queued = false;
                    }
                }
            }
        }

        private void UpdateSkills()
        {
            foreach(var kvp in SlottedSkills)
            {
                kvp.Value.Update();
            }
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
                var incSpeed = GameWorld.Instance.Character.GetAttribute(Core.AttributeName.MovementSpeed);

                if(incSpeed != 0)
                {
                    speed += incSpeed / 100f * BaseMoveSpeed;
                }

                Direction = (_waypoints[_currentWaypoint] - transform.position).normalized;
                Velocity = Direction * speed;
                transform.Translate(Velocity * Time.deltaTime);
            }
        }

    }
}
