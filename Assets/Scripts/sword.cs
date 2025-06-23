using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sword : MonoBehaviour
{
    public float damage = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        var enemyhealt = collision.gameObject.GetComponent<enemyhealt>();
        if(enemyhealt != null)
        {
            enemyhealt.DealDamage(damage);
        }
    }
}
