using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicCont : MonoBehaviour
{
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
            _audioSource.volume = Mathf.MoveTowards(_audioSource.volume, 0, 0.2f * Time.deltaTime);
            yield return null;
        }
        _audioSource.clip = clip;
        _audioSource.Play();
        while (_audioSource.volume < 1)
        {
            _audioSource.volume = Mathf.MoveTowards(_audioSource.volume, 1, 0.2f * Time.deltaTime);
            yield return null;
        }
        _coroutine = null;
    }
}