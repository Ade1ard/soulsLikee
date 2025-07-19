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

    private bool _inAttack = false;
    private float _timeLastSeen;
    private Animator _animator;
    private Rigidbody _enemy;
    
    [Header("PLayer")]
    private PlayerController _player;
    private bool _isPlayerNoticed;

    private NavMeshAgent _navMeshAgent;

    void Start()
    {
        _player = FindObjectOfType<PlayerController>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _enemy = GetComponent<Rigidbody>();
        PickNewTarget();
    }

    void Update()
    {
        CheckPlayerNotice();
        Movement();
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

    private void Movement()
    {
        if (_isPlayerNoticed)
        {
            var lookDirection = _player.transform.position - transform.position;
            lookDirection.y = 0;

            var DistanceToPLayer = Vector3.Distance(transform.position, _player.transform.position);

            if (DistanceToPLayer <= _fightDistance)
            {
                Debug.Log(DistanceToPLayer);
                _navMeshAgent.speed = _WalkSpeed;
                _navMeshAgent.ResetPath();
                _animator.SetFloat("Speed", 3);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), 10 * Time.deltaTime);
            }
            else if (DistanceToPLayer < _attackDistance || _inAttack)
            {
                Debug.Log(DistanceToPLayer);
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
        else
        {
            _inAttack = false;
            _navMeshAgent.speed = _WalkSpeed;
            _animator.SetFloat("Speed", 1);

            if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
            {
                PickNewTarget();
            }
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

    private void FixedUpdate()
    {
        if (_isPlayerNoticed && Vector3.Distance(transform.position, _player.transform.position) <= _fightDistance)
        {
            bool clockwise = true;
            float circleSpeed = 2f;

            Vector3 circleDirection = (transform.position - _player.transform.position).normalized;
            float angle = Mathf.Atan2(circleDirection.z, circleDirection.x) + (clockwise ? -1 : 1) * circleSpeed * Time.deltaTime;

            float x = _player.transform.position.x + Mathf.Cos(angle) * _fightDistance;
            float z = _player.transform.position.z + Mathf.Sin(angle) * _fightDistance;

            Vector3 targetPosition = new Vector3(x, transform.position.y, z);
            _enemy.MovePosition(targetPosition);
        }
    }
}
