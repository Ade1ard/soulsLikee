using UnityEngine.VFX;
using UnityEngine;

public class VFX_Destroer : MonoBehaviour
{
    private VisualEffect _effect;
    [SerializeField] private float _deathDelay = 3;
    void Start()
    {
        _effect = GetComponent<VisualEffect>();
        _effect.Play();
        Invoke("Destroy", _deathDelay);
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
