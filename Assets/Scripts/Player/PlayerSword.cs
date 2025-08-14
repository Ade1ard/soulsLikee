using System.Collections.Generic;
using UnityEngine;

public class PlayerSword : MonoBehaviour
{
    [SerializeField] private float _damage = 35;

    [SerializeField] private ParticleSystem _blood;
    [SerializeField] private ParticleSystem _sparkl;
    [SerializeField] private ParticleSystem _dust;

    [SerializeField] private List<AudioClip> _hitSounds;
    [SerializeField] private AudioClip _hitAtSomething;
    private AudioSource _audioSource;

    private CapsuleCollider _capsuleCollider;
    private bool _hasAttacked;
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _capsuleCollider.enabled = false;
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out EnemyHealth enemyHealth) && !_hasAttacked)
        {
            enemyHealth.TakeDamage(_damage);

            var ContactPoint = other.ClosestPoint(transform.position);
            Instantiate(_blood, ContactPoint, Quaternion.LookRotation(Vector3.forward));

            _audioSource.PlayOneShot(_hitSounds[Random.Range(0, _hitSounds.Count)]);

            _hasAttacked = true;
        }
        
        if (!other.CompareTag("Camera Ignore"))
        {
            _audioSource.PlayOneShot(_hitAtSomething);

            var ContactPoint = other.ClosestPoint(transform.position);
            Instantiate(_sparkl, ContactPoint, Quaternion.LookRotation(gameObject.transform.position));
            Instantiate(_dust, ContactPoint, Quaternion.LookRotation(gameObject.transform.position));
        }
    }

    public void AddDamage(float amount)
    {
        _damage += Mathf.Abs(amount);
    }

    public void EndAttack()
    {
        _capsuleCollider.enabled = false;
        _hasAttacked = false;
    }

    public void StartAttack()
    {
        _capsuleCollider.enabled = true;
    }
}
