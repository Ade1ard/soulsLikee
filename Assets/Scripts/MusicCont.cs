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
    private Coroutine _coroutine;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void ChangeCurrentSoundtrec(AudioClip clip)
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(ChangingCurrentSoundtrec(clip));
    }

    private IEnumerator ChangingCurrentSoundtrec(AudioClip clip)
    {
        while (_audioSource.volume > 0)
        {
            _audioSource.volume = Mathf.MoveTowards(_audioSource.volume, 0, 0.4f * Time.deltaTime);
            yield return null;
        }
        _audioSource.clip = clip;
        _audioSource.Play();
        while (_audioSource.volume < _targetVolume)
        {
            _audioSource.volume = Mathf.MoveTowards(_audioSource.volume, _targetVolume, 0.4f * Time.deltaTime);
            yield return null;
        }
        _coroutine = null;
    }
}