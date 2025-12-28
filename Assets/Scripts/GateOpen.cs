using System.Linq;
using UnityEngine;

public class GateOpen : MonoBehaviour, ISaveable
{
    [SerializeField] private GameObject _gateDoor1;
    [SerializeField] private GameObject _gateDoor2;

    [SerializeField] private string _tutClueString;

    private TutorialClueCont _tutorialClueCont;
    private GameObject _player;

    private bool _nearGate = false;
    private bool _isGateOpened = false;
    
    public virtual void Initialize(BootStrap bootStrap)
    {
        _tutorialClueCont = bootStrap.Resolve<TutorialClueCont>();
        _player = bootStrap.Resolve<PlayerController>().gameObject;
    }

    public void SaveTo(GameData gameData)
    {
        var gateData = gameData.gateDatas.FirstOrDefault(e => e.GateID == gameObject.name);

        if (gateData == null)
        {
            gateData = new GateGata();
            gameData.gateDatas.Add(gateData);
        }

        gateData.GateID = gameObject.name;
        gateData.IsGateOpen = _isGateOpened;
    }

    public void LoadFrom(GameData gameData)
    {
        var gateData = gameData.gateDatas.FirstOrDefault(e => e.GateID == gameObject.name);

        if (gateData != null)
        {
            if (gateData.IsGateOpen)
                OpenGate();
        }
    }

    protected virtual void Update()
    {
        if (_nearGate)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                OpenGate();
            }
        }
    }

    protected virtual void OpenGate()
    {
        _gateDoor1.GetComponent<Animator>().SetTrigger("Open");
        _gateDoor2.GetComponent<Animator>().SetTrigger("Open");

        _tutorialClueCont.TutorialGetUnvisible();

        _isGateOpened = true;
    }

    protected virtual void OnTriggerEnter(Collider other)
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
