using System.Collections.Generic;
using UnityEngine;

public class DeadUnitCont : MonoBehaviour
{
    [SerializeField] private List<GameObject> DeadUnitsMeshes = new List<GameObject>();

    void Start()
    {
        DeadUnitsMeshes[Random.Range(0, DeadUnitsMeshes.Count)].SetActive(true);

        Vector3 dir = new Vector3(Random.Range(0f, 360f), 0, Random.Range(0, 360));
        transform.rotation = Quaternion.LookRotation(dir);

        float Scale = Random.Range(0.9f, 1.7f);
        transform.localScale = new Vector3(Scale, Scale, Scale);
    }
}
