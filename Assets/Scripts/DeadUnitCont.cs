using UnityEngine;

public class DeadUnitCont : MonoBehaviour
{
    void Start()
    {
        Vector3 dir = new Vector3(Random.Range(0f, 360f), 0, Random.Range(0, 360));
        transform.rotation = Quaternion.LookRotation(dir);
        GetComponent<Animator>().SetFloat("Dead", Random.Range(0.1f, 4f));
    }
}
