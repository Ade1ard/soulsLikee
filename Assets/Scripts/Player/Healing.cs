using UnityEngine;

public class Healing : MonoBehaviour
{
    private Animator _animator;
    private PlayerHealth _health;

    [SerializeField] private float _healHPCount;
    [SerializeField] private GameObject _healFlask;
    void Start()
    {
        _animator = GetComponent<Animator>();
        _health = GetComponent<PlayerHealth>();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            _animator.SetTrigger("Heal");
        }
    }

    private void Heal() //called by events in animations
    {
        _health.AddHealt(_healHPCount);
    }

    private void IsHealing(int _int) //called by events in animations
    {
        if (_int == 1)
        {
            _healFlask.SetActive(true);
        }
        else
        {
            _healFlask.SetActive(false);
        }
    }
}
