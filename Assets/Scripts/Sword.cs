using UnityEngine;

public class Sword : MonoBehaviour
{
    private CapsuleCollider _capsuleCollider;
    private float _damage = 40;
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
        if (other.gameObject.GetComponent<EnemyHealth>())
        {
            var EnemyHealth = other.gameObject.GetComponent<EnemyHealth>();
            EnemyHealth.TakeDamage(_damage);
        }
    }

    public void AddDamage(float amount)
    {
        _damage += Mathf.Abs(amount);
    }

    public void EndAttack()
    {
        _capsuleCollider.enabled = false;
    }

    public void StartAttack()
    {
        _capsuleCollider.enabled = true;
    }
}
