using TMPro;
using UnityEngine;

public class Healing : MonoBehaviour
{
    [SerializeField] private float _FlaskCount;

    [SerializeField] private ParticleSystem _effect;

    [SerializeField] private GameObject _healFlask;
    [SerializeField] private TextMeshProUGUI _FlaskCountText;

    private float _healHPCount;

    private Animator _animator;
    private PlayerHealth _health;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _health = GetComponent<PlayerHealth>();
        _FlaskCountText.text = _FlaskCount.ToString();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            _animator.SetTrigger("Heal");
        }
    }

    public void GetFlaskEfficiency(float amount)
    {
        _healHPCount = Mathf.Abs(amount);
    }

    private void Heal() //called by events in animations
    {
        if (_FlaskCount > 0)
        {
            _health.AddHealt(_healHPCount);
            _FlaskCount -= 1;
            _FlaskCountText.text = _FlaskCount.ToString();
            Instantiate(_effect, transform.position + Vector3.up * 1.5f, Quaternion.identity);
        }
    }

    private void IsHealing(int _int) //called by events in animations
    {
        _healFlask.SetActive(_int == 1 ? true : false);
    }
}
