using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class ButtonSounds : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [SerializeField] private AudioClip _hoverSound;
    [SerializeField] private AudioClip _clickSound;
    private AudioSource _audioSource;

    public void Initialize(BootStrap bootStrap)
    {
        _audioSource = bootStrap.ResolveAll<AudioSource>().FirstOrDefault(e => e.name == "UI");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PlaySound(_hoverSound);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PlaySound(_clickSound);
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && _audioSource != null)
        {
            _audioSource.PlayOneShot(clip);
        }
    }
}