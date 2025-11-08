using UnityEngine;

public class GateOpen : MonoBehaviour
{
    [SerializeField] private GameObject _gateDoor1;
    [SerializeField] private GameObject _gateDoor2;

    [SerializeField] private string _tutClueString;

    private TutorialClueCont _tutorialClueCont;
    private GameObject _player;

    private bool _nearGate = false;
    private bool _isGateOpened = false;
    
    public void Initialize(BootStrap bootStrap)
    {
        _tutorialClueCont = bootStrap.Resolve<TutorialClueCont>();
        _player = bootStrap.Resolve<PlayerController>().gameObject;
    }

    private void Update()
    {
        if (_nearGate)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                OpenGate();
                _isGateOpened = true;

                this.enabled = false;
            }
        }
    }

    public void OpenGate()
    {
        _gateDoor1.GetComponent<Animator>().SetTrigger("Open");
        _gateDoor2.GetComponent<Animator>().SetTrigger("Open");

        _tutorialClueCont.TutorialGetUnvisible();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _player && !_isGateOpened)
        {
            _tutorialClueCont.TutorialGetVisible(_tutClueString);
            _nearGate = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == _player && !_isGateOpened)
        {
            _tutorialClueCont.TutorialGetUnvisible();
            _nearGate = false;
        }
    }
}
