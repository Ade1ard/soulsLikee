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
        Dissolve(false);
    }

    public Coroutine Dissolve(bool active, float ? dissolveDelay = null, float ? dissolveRate = null)
    {
        float actualDissolveRate = dissolveRate ?? _dissolveRate;
        float actualDissolveDelay = dissolveDelay ?? _dissolveDelay;


        if (_coroutine != null)
            StopCoroutine(_coroutine);
        return _coroutine = StartCoroutine(Dissolving(active, actualDissolveRate, actualDissolveDelay));
    }

    private IEnumerator Dissolving(bool active, float dissolveRate, float dissolveDelay)
    {
        float target = active? 0 : 1;

        while (_material.GetFloat("_DissolveAmount") != target)
        {
            _material.SetFloat("_DissolveAmount", Mathf.MoveTowards(_material.GetFloat("_DissolveAmount"), target, dissolveRate));

            yield return new WaitForSeconds(dissolveDelay);
        }
    }
}
