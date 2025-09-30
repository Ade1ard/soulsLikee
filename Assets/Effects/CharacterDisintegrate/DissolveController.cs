using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class DissolveController : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer[] _skinnedMeshes;

    [SerializeField] private VisualEffect _VFXGraph;

    [SerializeField] private float _dissolveRate = 0.0125f;
    [SerializeField] private float _dissolveDelay = 0.025f;

    List<Material> _skinnedMaterials = new List<Material>();
    void Start()
    {
        _VFXGraph.Stop();

        if (_skinnedMeshes != null)
        {
            foreach(SkinnedMeshRenderer skinnedMesh in _skinnedMeshes)
            {
                
                foreach(Material material in skinnedMesh.materials)
                {
                    _skinnedMaterials.Add(material);
                }
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
        if (_VFXGraph != null)
        {
            _VFXGraph.Play();
        }

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
