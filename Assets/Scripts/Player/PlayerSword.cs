using System.Collections.Generic;
using UnityEngine;

public class PlayerSword : MonoBehaviour
{
    [Header("Effects")]
    [SerializeField] private ParticleSystem _sparkl;
    [SerializeField] private ParticleSystem _swordTrail;

    [Header("Sounds")]
    [SerializeField] private List<AudioClip> _hitSounds;
    [SerializeField] private AudioClip _hitAtSomething;
    private AudioSource _audioSource;

    private float _damage;

    private CapsuleCollider _capsuleCollider;
    private BloodVFXController _Blood;

    private bool _hasAttacked;
    public void Initialize(BootStrap bootStrap)
    {
        _Blood = bootStrap.Resolve<BloodVFXController>();
        _audioSource = GetComponent<AudioSource>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _capsuleCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out EnemyHealth enemyHealth) && !_hasAttacked)
        {
            if (enemyHealth.CheckAlive())
            {
                enemyHealth.TakeDamage(_damage);

                var ContactPoint = other.ClosestPoint(transform.position);
                _Blood.SpawnVFXBlood(ContactPoint, transform.position);

                _audioSource.PlayOneShot(_hitSounds[Random.Range(0, _hitSounds.Count)]);

                _hasAttacked = true;
            }
        }
        
        if (other.CompareTag("Environment") || other.CompareTag("EnemySword"))
        {
            _audioSource.PlayOneShot(_hitAtSomething);

            if (other is MeshCollider)
            {
                var ContactPoint = transform.position;
                Instantiate(_sparkl, ContactPoint, Quaternion.LookRotation(gameObject.transform.position));
            }
            else
            {
                var ContactPoint = other.ClosestPoint(transform.position);
                Instantiate(_sparkl, ContactPoint, Quaternion.LookRotation(gameObject.transform.position));
            }
        } 
    }

    public void GetDamageValue(float amount)
    {
        _damage = Mathf.Abs(amount);
    }

    public void EndAttack()
    {
        _capsuleCollider.enabled = false;
        _hasAttacked = false;
        _swordTrail.Stop();
    }

    public void StartAttack()
    {
        _capsuleCollider.enabled = true;
        _swordTrail.Play();
    }
}
