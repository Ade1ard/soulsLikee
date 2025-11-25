using UnityEngine;

public class MessageTut : MonoBehaviour
{
    [SerializeField] private string _tutorialClueText;
    [SerializeField] private string _messageText;

    private GameObject _player;
    private TutorialClueCont _tutorialClueCont;
    private MessageUI _messageUI;

    private bool _isNear = false;

    public void Initialize(BootStrap bootStrap)
    {
        _player = bootStrap.Resolve<PlayerController>().gameObject;
        _tutorialClueCont = bootStrap.Resolve<TutorialClueCont>();
        _messageUI = bootStrap.Resolve<MessageUI>();
    }

    private void Update()
    {
        if (_isNear)
        {
            if (Input.GetKeyUp(KeyCode.F))
            {
                _tutorialClueCont.TutorialGetUnvisible();
                _messageUI.MessageGetVisible(_messageText);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == _player)
        {
            _isNear = true;
            _tutorialClueCont.TutorialGetVisible(_tutorialClueText);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == _player)
        {
            _isNear = false;
            _tutorialClueCont.TutorialGetUnvisible();
            _messageUI.MessageGetUnvisible();
        }
    }
}
