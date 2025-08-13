using UnityEngine;

public class EnemySword : MonoBehaviour
{
    [SerializeField] private float _minDamage = 25;
    [SerializeField] private float _maxDamage = 45;

    [SerializeField] private ParticleSystem _Blood;

    private CapsuleCollider _capsuleCollider;
    private bool _hasAttacked;

    void Start()
    {
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _capsuleCollider.enabled = false;
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerHealth playerHealth) && !_hasAttacked)
        {
            playerHealth.DealDamage(Random.Range(_minDamage, _maxDamage));

            var ContactPoint = other.ClosestPoint(transform.position);
            Instantiate(_Blood, ContactPoint, Quaternion.LookRotation(Vector3.forward));

            _hasAttacked = true;
        }
    }

    public void StartAttack()
    {
        _capsuleCollider.enabled = true;
    }

    public void EndAttack()
    {
        _capsuleCollider.enabled = false;
        _hasAttacked = false;
    }
}
