using UnityEngine;

public class VFXToHips : MonoBehaviour
{
    [SerializeField] private Transform _hips;
    void Start()
    {
        
    }

    void Update()
    {
        transform.position = _hips.position;
    }
}
