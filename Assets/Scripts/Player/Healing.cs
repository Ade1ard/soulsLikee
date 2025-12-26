using System.Linq;
using TMPro;
using UnityEngine;

public class Healing : MonoBehaviour, ISaveable, IRebootable
{
    [SerializeField] private float _FlaskCount;
    private float _maxFlaskCount;

    [SerializeField] private ParticleSystem _effect;

    [SerializeField] private GameObject _healFlask;
    [SerializeField] private TextMeshProUGUI _FlaskCountText;

    [SerializeField] private AudioClip _healSound;

    private float _healHPCount;

    private bool _isHealing = false;

    private Animator _animator;
    private PlayerHealth _health;
    private AudioSource _audioSource;

    public void Initialize(BootStrap bootStrap)
    {
        _animator = bootStrap.ResolveAll<Animator>().FirstOrDefault(e => e.name == "Player");
        _health = bootStrap.Resolve<PlayerHealth>();
        _FlaskCountText.text = _FlaskCount.ToString();
        _maxFlaskCount = _FlaskCount;

        _audioSource = GetComponent<AudioSource>();
    }

    public void SaveTo(GameData gameData)
    {
        gameData.FlaskCount = _FlaskCount;
    }

    public void LoadFrom(GameData gameData)
    {
        _FlaskCount = gameData.FlaskCount;
        _FlaskCountText.text = _FlaskCount.ToString();
    }

    public void Reboot()
    {
        _FlaskCount = _maxFlaskCount;
        _FlaskCountText.text = _FlaskCount.ToString();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.R) && !_isHealing)
        {
            _animator.SetTrigger("Heal");
            _isHealing = true;
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
            _audioSource.PlayOneShot(_healSound);
            _health.AddHealt(_healHPCount);
            _FlaskCount -= 1;
            _FlaskCountText.text = _FlaskCount.ToString();
            Instantiate(_effect, transform.position + Vector3.up * 1.5f, Quaternion.identity);
        }
    }

    private void IsHealing(int _int) //called by events in animations
    {
        _healFlask.SetActive(_int == 1 ? true : false);
        _isHealing = _int == 1 ? true : false;
    }
}
