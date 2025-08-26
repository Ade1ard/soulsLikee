using System.Collections.Generic;
using UnityEngine;

public class EnemySword : MonoBehaviour
{
    [SerializeField] private float _minDamage = 25;
    [SerializeField] private float _maxDamage = 45;

    [SerializeField] private ParticleSystem _Blood;
    [SerializeField] private ParticleSystem _SworsTrail;

    [SerializeField] private List<AudioClip> _hitSounds;
    private AudioSource _audioSource;

    private CapsuleCollider _capsuleCollider;
    private PlayerHealth _playerHealth;
    private bool _hasAttacked;

    void Start()
    {
        _playerHealth = FindObjectOfType<PlayerHealth>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _audioSource = GetComponent<AudioSource>();
        _capsuleCollider.enabled = false;
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("HitBox") && !_hasAttacked)
        {
            _playerHealth.DealDamage(Random.Range(_minDamage, _maxDamage));

            var ContactPoint = other.ClosestPoint(transform.position);
            Instantiate(_Blood, ContactPoint, Quaternion.LookRotation(Vector3.forward));

            _audioSource.PlayOneShot(_hitSounds[Random.Range(0, _hitSounds.Count)]);

            _hasAttacked = true;
        }
    }

    public void StartAttack()
    {
        _capsuleCollider.enabled = true;
        _SworsTrail.Play();
    }

    public void EndAttack()
    {
        _capsuleCollider.enabled = false;
        _hasAttacked = false;
        _SworsTrail.Stop();
    }
}
