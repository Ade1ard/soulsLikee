using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemtake : MonoBehaviour
{
    public GameObject UItake;
    public float takeDistanse = 3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Vector3 center = new Vector3(Screen.width / 2, Screen.height / 2);
        Ray ray = Camera.main.ScreenPointToRay(center);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("CanTake") && hit.distance <= takeDistanse)
            {
                UItake.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E))
                {
                   Destroy(hit.collider.gameObject);
                }
            }
            else
            {
               UItake.SetActive(false);
            }
        }
    }
}
