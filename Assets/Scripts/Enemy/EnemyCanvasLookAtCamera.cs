using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class EnemyCanvasLookAtCamera : MonoBehaviour
{
    [SerializeField] private Transform _cameraTransform;

    void Update()
    {
        transform.LookAt(_cameraTransform.position);
    }
}
