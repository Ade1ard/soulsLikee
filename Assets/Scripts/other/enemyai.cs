using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyai : MonoBehaviour
{
    public List<Transform> targetpoints;
    public PlayerController player;
    public float viewAngle;
    public float damage = 20;

    public float attackdistanse = 1;

    private NavMeshAgent _navMeshAgent;
    private bool _isPlayerNoticed;
    private playerHealt _playerHealt;
    private enemyhealt _enemyhealt;
    private CharacterController PlayerCont;

    public Animator _animatorEnemy;



    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();

        PickNewTargetPoint();
        
        _playerHealt = player.GetComponent<playerHealt>();

        PlayerCont = player.GetComponent<CharacterController>();
    }

    void Update()
    {
        if (!_isPlayerNoticed)
        {
            if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
            {
                PickNewTargetPoint();
            }
        }
       
        _isPlayerNoticed = false;
        if (_playerHealt.value <= 0) return;

        var direction = player.transform.position - transform.position;

        if (Vector3.Angle(transform.forward, direction) < viewAngle)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position + Vector3.up, direction, out hit))
            {
                if (hit.collider.gameObject == player.gameObject)
                {
                    _isPlayerNoticed = true;
                }
            }
        }

        

        if (_isPlayerNoticed)
        {
            _navMeshAgent.destination = player.transform.position;
        }

        AttackUpdate();
    }
    private void PickNewTargetPoint()
    {
        _navMeshAgent.destination = targetpoints[Random.Range(0, targetpoints.Count)].position;
    }

    public void AttackDamageEvent()
    {
        if (!_isPlayerNoticed) return;
        if (_navMeshAgent.remainingDistance > (_navMeshAgent.stoppingDistance + attackdistanse)) return;
        _playerHealt.DealDamage(damage);
    }


    private void AttackUpdate()
    {
        if (_isPlayerNoticed)
        {
            if(_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance && PlayerCont.isGrounded)
            {
                _animatorEnemy.SetTrigger("attack");
            }
        }
    }
}        
