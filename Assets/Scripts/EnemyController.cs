using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("EnemySettings")]
    [SerializeField] private List<Transform> _targetPoints;
    [SerializeField] private float _viewAngle;
    [SerializeField] private float _attackDisatanse;
    [SerializeField] private float _noticeDisatanse;
    [SerializeField] private float _runToPlayerSpeed;
    [SerializeField] private float _WalkSpeed;
    private bool _inAttack = false;
    
    [Header("PLayer")]
    private PlayerController _player;
    private bool _isPlayerNoticed;

    private NavMeshAgent _navMeshAgent;

    void Start()
    {
        _player = FindObjectOfType<PlayerController>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        PickNewTarget();
    }

    void Update()
    {

        _isPlayerNoticed = false;

        var direction = _player.transform.position - transform.position;
        if (Vector3.Angle(transform.forward, direction) < _viewAngle)
        {
            RaycastHit hit;
            if(Physics.Raycast(transform.position + Vector3.up, direction, out hit))
            {
                if(hit.collider.gameObject == _player.gameObject && Vector3.Distance(transform.position, _player.transform.position) < _noticeDisatanse)
                {
                    _isPlayerNoticed = true;
                }
            }
        }

        if (_isPlayerNoticed)
        {
            _navMeshAgent.speed = _runToPlayerSpeed;

            if (Vector3.Distance(transform.position, _player.transform.position) < _attackDisatanse || _inAttack)
            {
                _inAttack = true;
                _navMeshAgent.destination = _player.transform.position;
            }
            else
            {
                _navMeshAgent.destination = transform.position;
                var lookDirection = _player.transform.position - transform.position;
                lookDirection.y = 0;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), 10 * Time.deltaTime);
            }
        }
        else
        {
            _inAttack = false;
            _navMeshAgent.speed = _WalkSpeed;

            if(_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
            {
                if(_targetPoints.Count != 0)
                {
                    PickNewTarget();
                }
                else
                {
                    _navMeshAgent.destination = transform.position;
                }
            }
        }
    }

    private void PickNewTarget()
    {
        _navMeshAgent.destination = _targetPoints[Random.Range(0, _targetPoints.Count)].position;
    }
}
