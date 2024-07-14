using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemraycast : MonoBehaviour
{
    public List<Transform> items;
    public float viewAngle = 90;
    public float viewDistanse = 2;
    public GameObject UItake;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var ite = items[Random.Range(0, items.Count)];
        var direction = ite.transform.position - transform.position;

        if (Vector3.Angle(transform.forward, direction) < viewAngle)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position + Vector3.up, direction, out hit))
            {
                if (hit.collider.gameObject == ite.gameObject)
                {
                    if (hit.distance <= viewDistanse)
                    {
                        Debug.Log("kmdfsvc");
                        UItake.SetActive(true);
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            Destroy(ite);
                            
                        }
                    }
                }
            }
        }
    }
}
