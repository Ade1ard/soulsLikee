using TMPro;
using UnityEngine;

public class Healing : MonoBehaviour
{
    [SerializeField] private float _FlackCount;

    [SerializeField] private float _healHPCount;
    [SerializeField] private GameObject _healFlask;
    [SerializeField] private TextMeshProUGUI _FlackCountText;

    private Animator _animator;
    private PlayerHealth _health;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _health = GetComponent<PlayerHealth>();
        _FlackCountText.text = _FlackCount.ToString();
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
        if (_FlackCount > 0)
        {
            _health.AddHealt(_healHPCount);
            _FlackCount -= 1;
            _FlackCountText.text = _FlackCount.ToString();
        }
    }

    private void IsHealing(int _int) //called by events in animations
    {
        _healFlask.SetActive(_int == 1 ? true : false);
    }
}
