using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveController : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer[] _skinnedMeshes;
    [SerializeField] private float _dissolveRate = 0.0125f;
    [SerializeField] private float _dissolveDelay = 0.025f;

    List<Material> _skinnedMaterials = new List<Material>();
    void Start()
    {
        if (_skinnedMeshes != null)
        {
            foreach(SkinnedMeshRenderer skinnedMesh in _skinnedMeshes)
            {
                _skinnedMaterials.Add(skinnedMesh.material);
            }
        }
    }

    void Update()
    {
        
    }

    public void Dissolve()
    {
        StartCoroutine(DissolveCorutine());
    }

    IEnumerator DissolveCorutine()
    {
        if (_skinnedMaterials.Count > 0)
        {
            float counter = 0;

            while (_skinnedMaterials[0].GetFloat("_DissolveAmount") < 1)
            {
                counter += _dissolveRate;

                foreach (Material skinnedMaterial in _skinnedMaterials)
                {
                    skinnedMaterial.SetFloat("_DissolveAmount", counter);
                }
                yield return new WaitForSeconds(_dissolveDelay);
            }
        }
    }
}
