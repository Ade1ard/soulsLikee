using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicCont : MonoBehaviour
{
    [SerializeField] [Range(0,1)] private float _targetVolume;

    [Header("Music")]
    public AudioClip _bossFigthSoundtrec;
    public AudioClip _standartSoundtrec;

    private AudioSource _audioSource;
    private Coroutine _FadeCoroutine;
    private Coroutine _IncreaseCoroutine;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void ChangeCurrentSoundtrec(AudioClip clip)
    {
        if (_FadeCoroutine != null)
            StopCoroutine(_FadeCoroutine);
        _FadeCoroutine = StartCoroutine(FadeCurrentSoundtrec());

        _audioSource.clip = clip;
        _audioSource.Play();

        if (_IncreaseCoroutine != null)
            StopCoroutine(_IncreaseCoroutine);
        _IncreaseCoroutine = StartCoroutine(IncreaseCurrentSoundtrec());
    }

    public IEnumerator FadeCurrentSoundtrec()
    {
        while (_audioSource.volume > 0)
        {
            _audioSource.volume = Mathf.MoveTowards(_audioSource.volume, 0, 0.3f * Time.deltaTime);
            yield return null;
        }
        _FadeCoroutine = null;
    }

    public IEnumerator IncreaseCurrentSoundtrec()
    {
        while (_audioSource.volume < _targetVolume)
        {
            _audioSource.volume = Mathf.MoveTowards(_audioSource.volume, _targetVolume, 0.3f * Time.deltaTime);
            yield return null;
        }
        _IncreaseCoroutine = null;
    }
}