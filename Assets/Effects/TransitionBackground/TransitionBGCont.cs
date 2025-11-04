using System.Collections;
using UnityEngine;

public class TransitionBGCont : MonoBehaviour
{
    [SerializeField] private Material _material;

    [SerializeField] private float _dissolveRate = 0.0125f;
    [SerializeField] private float _dissolveDelay = 0.025f;

    private Coroutine _coroutine;

    private void Start()
    {
        _coroutine = StartCoroutine(Dissolving(false));
    }

    public Coroutine Dissolve(bool active)
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
        return _coroutine = StartCoroutine(Dissolving(active));
    }

    private IEnumerator Dissolving(bool active)
    {
        float target = active? 0 : 1;

        while (_material.GetFloat("_DissolveAmount") != target)
        {
            _material.SetFloat("_DissolveAmount", Mathf.MoveTowards(_material.GetFloat("_DissolveAmount"), target, _dissolveRate));

            yield return new WaitForSeconds(_dissolveDelay);
        }
    }
}
