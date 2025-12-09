using UnityEngine;

public class FogCollider : MonoBehaviour
{
    private BoxCollider _fogCollider;

    private void Start()
    {
        _fogCollider = GetComponent<BoxCollider>();
        _fogCollider.isTrigger = true;
    }

    public void ActivateCollider() { _fogCollider.isTrigger = false; }
}
