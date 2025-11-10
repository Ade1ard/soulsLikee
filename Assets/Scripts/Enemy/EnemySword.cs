using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySword : MonoBehaviour
{
    [Header("Floats")]
    [SerializeField] private float _minDamage = 25;
    [SerializeField] private float _maxDamage = 45;

    [Header("Effects")]
    [SerializeField] private ParticleSystem _SworsTrail;

    [Header("Sounds")]
    [SerializeField] private List<AudioClip> _hitSounds;
    private AudioSource _audioSource;

    private CapsuleCollider _capsuleCollider;
    private PlayerHealth _playerHealth;
    private BloodVFXController _blood;
    private bool _hasAttacked;

    public void Initialize(BootStrap bootStrap)
    {
        _blood = bootStrap.Resolve<BloodVFXController>();
        _playerHealth = bootStrap.Resolve<PlayerHealth>();
        _audioSource = bootStrap.ResolveAll<AudioSource>().FirstOrDefault(e => e.name == gameObject.name);

        _capsuleCollider = GetComponent<CapsuleCollider>();
        _capsuleCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("HitBox") && !_hasAttacked && !_playerHealth.CheckInvulnerability())
        {
            _playerHealth.DealDamage(Random.Range(_minDamage, _maxDamage));

            var ContactPoint = other.ClosestPoint(transform.position);
            _blood.SpawnVFXBlood(ContactPoint, transform.position);

            _audioSource.PlayOneShot(_hitSounds[Random.Range(0, _hitSounds.Count)]);

            _hasAttacked = true;
        }
    }

    public void StartAttack()
    {
        _capsuleCollider.enabled = true;
        _SworsTrail.Play();
    }

    public void KomboCanDamage()
    {
        _hasAttacked = false;
    }

    public void EndAttack()
    {
        _capsuleCollider.enabled = false;
        _hasAttacked = false;
        _SworsTrail.Stop();
    }
}
