using UnityEngine;

public class SetColliderVFXBlood : MonoBehaviour
{
    [SerializeField] private BoxCollider _groundCollider;

    private BloodVFXController _bloodCont;
    private GameObject _player;

    public void Initialize(BootStrap bootStrap)
    {
        _bloodCont = bootStrap.Resolve<BloodVFXController>();
        _player = bootStrap.Resolve<PlayerController>().gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _player)
            _bloodCont.SetCurrentCollider(_groundCollider);
    }
}
