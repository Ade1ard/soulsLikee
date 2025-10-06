using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class EnemyCanvasLookAtCamera : MonoBehaviour
{
    private Transform _cameraTransform;

    public void Initialize(BootStrap bootStrap)
    {
        _cameraTransform = bootStrap.Resolve<CinemachineBrain>().transform;
    }

    void Update()
    {
        transform.LookAt(_cameraTransform.position);
    }
}
