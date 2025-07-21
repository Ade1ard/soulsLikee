using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("EnemySettings")]
    [SerializeField] private List<Transform> _targetPoints;
    [SerializeField] private float _viewAngle;

    [SerializeField] private float _runToPlayerSpeed;
    [SerializeField] private float _WalkSpeed;

    [SerializeField] private float _attackDistance;
    [SerializeField] private float _noticeDistance;
    [SerializeField] private float _fightDistance;

    [SerializeField] private float _attackDelay;
    private bool _inAttack = false;
    private float _timeLastAttack;
    private float _timeLastSeen;
    private Animator _animator;
    
    private NavMeshAgent _navMeshAgent;

    [Header("PLayer")]
    private PlayerController _player;
    private bool _isPlayerNoticed;


    void Start()
    {
        _player = FindObjectOfType<PlayerController>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();

        PickNewTarget();
    }

    void Update()
    {
        CheckPlayerNotice();

        if (_isPlayerNoticed)
        {
            MovementToPLayer();
        }
        else
        {
            Patrolling();
        }
    }

    private void CheckPlayerNotice()
    {
        var direction = _player.transform.position - transform.position;
        if (Vector3.Angle(transform.forward, direction) < _viewAngle)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position + Vector3.up, direction, out hit, _noticeDistance))
            {
                if (hit.collider.gameObject == _player.gameObject)
                {
                    _isPlayerNoticed = true;
                    _timeLastSeen = Time.time;
                }
            }
        }
        
        if (Time.time - _timeLastSeen > 3)
        {
            _isPlayerNoticed = false;
        }
    }

    private void MovementToPLayer()
    {
        var lookDirection = _player.transform.position - transform.position;
            lookDirection.y = 0;

        var DistanceToPLayer = _navMeshAgent.remainingDistance;

        if (DistanceToPLayer <= _fightDistance)
        {
            Fight();
        }
        else if (DistanceToPLayer < _attackDistance || _inAttack)
        {
            _navMeshAgent.speed = _runToPlayerSpeed;
            _animator.SetFloat("Speed", 2);

            _inAttack = true;
            _navMeshAgent.destination = _player.transform.position;
        }
        else
        {
            _navMeshAgent.destination = transform.position;
            _animator.SetFloat("Speed", -1);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), 10 * Time.deltaTime);
        }
    }

    private void Fight()
    {
        if (Time.time - _timeLastAttack > _attackDelay)
        {
            if (_navMeshAgent.remainingDistance > _navMeshAgent.stoppingDistance + 0.2f)
            {
                _animator.SetFloat("AttackFar", Random.Range(1, 5));
                _timeLastAttack = Time.time;
            }
            else
            {
                //_animator.SetFloat("AttackNear", Random.Range(1, 3));
                _animator.SetFloat("AttackFar", Random.Range(1, 5));
                _timeLastAttack = Time.time;
            }
            _navMeshAgent.ResetPath();
        }
        else
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                _navMeshAgent.destination = _player.transform.position;
                _navMeshAgent.speed = _WalkSpeed;
                _animator.SetFloat("Speed", 1);
            }
            _animator.SetFloat("AttackFar", -1);
            _animator.SetFloat("AttackNear", -1);
            
            var lookDirection = _player.transform.position - transform.position;
            lookDirection.y = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), 10 * Time.deltaTime);
        }
    }

    private void Patrolling()
    {
        _inAttack = false;
        _navMeshAgent.speed = _WalkSpeed;
        _animator.SetFloat("Speed", 1);

        if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        {
            PickNewTarget();
        }
    }

    private void PickNewTarget()
    {
        if (_targetPoints.Count != 0)
        {
            _navMeshAgent.destination = _targetPoints[Random.Range(0, _targetPoints.Count)].position;
        }
        else
        {
            _navMeshAgent.destination = transform.position;
            _animator.SetFloat("Speed", -1);
        }
    }
}
