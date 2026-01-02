using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicCont : MonoBehaviour
{
    [SerializeField] [Range(0,1)] private float _targetVolume;

    [Header("Music")]
    public AudioClip _bossFigthSoundtrac;
    public List<AudioClip> _standartSoundtrac;

    public bool _inBossFight;

    private AudioSource _audioSource;
    private Coroutine _fadeCoroutine;
    private Coroutine _increaseCoroutine;
    private Coroutine _changingSoundtracCoroutine;
    private Coroutine _waitForEndCoroutine;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.loop = false;

        _audioSource.clip = (_standartSoundtrac[Random.Range(0, _standartSoundtrac.Count)]);
        _audioSource.Play();

        WaitForEnd();
    }

    public void ChangeCurrentSoundtrac(AudioClip clip)
    {
        if (_changingSoundtracCoroutine != null)
            StopCoroutine(_changingSoundtracCoroutine);
        _changingSoundtracCoroutine = StartCoroutine(ChangingSoundtrac(clip));
    }

    private IEnumerator ChangingSoundtrac(AudioClip clip)
    {
        if (_fadeCoroutine != null)
            StopCoroutine(_fadeCoroutine);
        _fadeCoroutine = StartCoroutine(FadeCurrentSoundtrac());
        yield return _fadeCoroutine;

        _audioSource.clip = clip;
        _audioSource.Play();
        WaitForEnd();

        if (_increaseCoroutine != null)
            StopCoroutine(_increaseCoroutine);
        _increaseCoroutine = StartCoroutine(IncreaseCurrentSoundtrac());
    }

    private void ChooseSoundtrac()
    {
        if (!_inBossFight)
        {
            _audioSource.clip = (_standartSoundtrac[Random.Range(0, _standartSoundtrac.Count)]);
            _audioSource.Play();
        }
        else
        {
            _audioSource.clip = (_bossFigthSoundtrac);
            _audioSource.Play();
        }

        WaitForEnd();

        Debug.Log("soundtrac changed");
    }

    public IEnumerator FadeCurrentSoundtrac()
    {
        while (_audioSource.volume > 0)
        {
            _audioSource.volume = Mathf.MoveTowards(_audioSource.volume, 0, 0.3f * Time.deltaTime);
            yield return null;
        }
        _fadeCoroutine = null;
    }

    public IEnumerator IncreaseCurrentSoundtrac()
    {
        while (_audioSource.volume < _targetVolume)
        {
            _audioSource.volume = Mathf.MoveTowards(_audioSource.volume, _targetVolume, 0.3f * Time.deltaTime);
            yield return null;
        }
        _increaseCoroutine = null;
    }

    private void WaitForEnd()
    {
        if (_waitForEndCoroutine != null)
            StopCoroutine(_waitForEndCoroutine);
        _waitForEndCoroutine = StartCoroutine(WaitforEnd());
    }

    private IEnumerator WaitforEnd()
    {
        yield return new WaitForSeconds(_audioSource.clip.length);
        ChooseSoundtrac();
    }
}