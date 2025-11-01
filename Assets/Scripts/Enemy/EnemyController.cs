using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("EnemySettings")]
    [SerializeField] private List<Transform> _targetPoints;
    [SerializeField] private float _viewAngle;

    [Header("Speeds")]
    [SerializeField] private float _runToPlayerSpeed;
    [SerializeField] private float _WalkSpeed;
    [SerializeField] private float _changeAnimationsSpeed = 2.5f;

    [Header("Distances")]
    [SerializeField] private float _attackDistance;
    [SerializeField] private float _noticeDistance;
    [SerializeField] private float _fightDistance;

    [Header("Attack")]
    [SerializeField] private float _lowRangeAttackDelay;
    [SerializeField] private float _highRangeAttackDelay;
    [SerializeField] private float _AttacksAnimationCount;

    [Header("Sounds")]
    [SerializeField] private List<AudioClip> _swordSwingSounds;

    private float _attackDelay;

    private bool _inAggression = false;
    private float _timeLastAttack;
    private float _timeLastSeen;
    private Animator _animator;
    private AudioSource _audioSource;
    private EnemySword _enemySword;

    private NavMeshAgent _navMeshAgent;

    private PlayerController _player;
    private bool _isPlayerNoticed;

    [Header("Objects for camera")]
    [SerializeField] private Transform _CameraLookAt;
    public Transform _cameraLookAt => _CameraLookAt;
    public Transform _enemySpine { get; private set; }


    public void Initialize(BootStrap bootStrap)
    {
        _player = bootStrap.Resolve<PlayerController>();
        _audioSource = bootStrap.ResolveAll<AudioSource>().FirstOrDefault(e => e.name == gameObject.name);
        _navMeshAgent = bootStrap.ResolveAll<NavMeshAgent>().FirstOrDefault(e => e.name == gameObject.name);
        _animator = bootStrap.ResolveAll<Animator>().FirstOrDefault(e => e.name == gameObject.name);
        _enemySword = bootStrap.ResolveAll<EnemySword>().FirstOrDefault(e => e.transform.root.name == gameObject.name);

        _enemySpine = _animator.GetBoneTransform(HumanBodyBones.Spine);
    }

    private void Start()
    {
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

        var DistanceToPLayer = Vector3.Distance(transform.position, _player.transform.position);

        if (DistanceToPLayer <= _fightDistance)
        {
            Fight();
        }
        else if (DistanceToPLayer < _attackDistance || _inAggression)
        {
            _navMeshAgent.speed = _runToPlayerSpeed;
            _animator.SetFloat("Speed", Mathf.Lerp(_animator.GetFloat("Speed"), 2, _changeAnimationsSpeed * Time.deltaTime));

            _inAggression = true;
            _navMeshAgent.destination = _player.transform.position;
        }
        else
        {
            _navMeshAgent.destination = transform.position;
            _animator.SetFloat("Speed", Mathf.Lerp(_animator.GetFloat("Speed"), 0, _changeAnimationsSpeed * Time.deltaTime));
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), 10 * Time.deltaTime);
        }
    }

    private void Fight()
    {
        if (Time.time - _timeLastAttack > _attackDelay)
        {
            _navMeshAgent.destination = _player.transform.position;
            if(Vector3.Distance(transform.position, _player.transform.position) <= _navMeshAgent.stoppingDistance + 0.15f)
            {
                _attackDelay = Random.Range(_lowRangeAttackDelay, _highRangeAttackDelay);
                _timeLastAttack = Time.time;
                _navMeshAgent.ResetPath();
                _animator.SetFloat("Attack", Random.Range(0.1f, _AttacksAnimationCount + 1));
                _animator.SetFloat("Speed", Mathf.Lerp(_animator.GetFloat("Speed"), 0, _changeAnimationsSpeed * Time.deltaTime));
            }
            else
            {
                _animator.SetFloat("Speed", Mathf.Lerp(_animator.GetFloat("Speed"), 2, _changeAnimationsSpeed * Time.deltaTime));
                _navMeshAgent.destination = _player.transform.position;
                _navMeshAgent.speed = _runToPlayerSpeed;
            }
        }
        else
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Movement")) // not in attack animation
            {
                RaycastHit hit;
                if (Physics.Raycast(_player.transform.position + Vector3.up, transform.position + Vector3.up, out hit, _fightDistance - 2f))
                {
                    _navMeshAgent.destination = hit.transform.position;
                }
                else
                {
                    Ray ray = new Ray(_player.transform.position, transform.position);
                    _navMeshAgent.destination = ray.GetPoint(_fightDistance - 2f); 
                }
                _navMeshAgent.speed = _WalkSpeed;
                if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
                {
                    _animator.SetFloat("Speed", Mathf.Lerp(_animator.GetFloat("Speed"), 0, _changeAnimationsSpeed * Time.deltaTime));
                }
                else
                {
                    _animator.SetFloat("Speed", Mathf.Lerp(_animator.GetFloat("Speed"), -1, _changeAnimationsSpeed * Time.deltaTime));
                }
            }
            else
            {
                _navMeshAgent.speed = _WalkSpeed;
                _navMeshAgent.destination = _player.transform.position;
            }
            _animator.SetFloat("Attack", -1);
        }

        var lookDirection = _player.transform.position - transform.position;
        lookDirection.y = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), 10 * Time.deltaTime);
    }

    private void Patrolling()
    {
        _inAggression = false;
        _navMeshAgent.speed = _WalkSpeed;
        _animator.SetFloat("Speed", Mathf.Lerp(_animator.GetFloat("Speed"), 1, _changeAnimationsSpeed * Time.deltaTime));

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
            _animator.SetFloat("Speed", Mathf.Lerp(_animator.GetFloat("Speed"), 0, _changeAnimationsSpeed * Time.deltaTime));
        }
    }

    private void StartAttack() //called by events in animations
    {
        _enemySword.StartAttack();
        _animator.speed = 1.2f;
        _audioSource.PlayOneShot(_swordSwingSounds[Random.Range(0, _swordSwingSounds.Count)]);
    }

    private void KomboCanDamage() //called by events in animations
    {
        _enemySword.KomboCanDamage();
        _audioSource.PlayOneShot(_swordSwingSounds[Random.Range(0, _swordSwingSounds.Count)]);
    }

    private void EndAttack() //called by events in animations
    {
        _enemySword.EndAttack();
        _animator.speed = 1f;
    }

    public void PlayerNoticedAfterHit()
    {
        _isPlayerNoticed = true;
    }

    public void Reboot()
    {
        _isPlayerNoticed = false;
        _navMeshAgent.ResetPath();
    }

    public bool CheckAlive()
    {
        return !_animator.GetCurrentAnimatorStateInfo(0).IsName("Death");
    }
}
