using UnityEngine;

public class BonFireCont : MonoBehaviour
{
    [SerializeField] private Animator _playerAnimator;

    private PlayerController _playerController;

    private bool _NearBonFire;

    void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        if (_NearBonFire)
        {
            if (Input.GetKeyUp(KeyCode.F))
            {
                _playerAnimator.SetTrigger("BonFireSitDown");
                _playerController.IsHealing(1);
            }

            if (Input.GetKeyUp(KeyCode.Escape))
            {
                _playerAnimator.SetTrigger("BonFireStandUp");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        _NearBonFire = true;
    }

    private void OnTriggerExit(Collider other)
    {
        _NearBonFire = false;
    }
}
