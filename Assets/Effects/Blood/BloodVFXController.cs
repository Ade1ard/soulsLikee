using UnityEngine.VFX;
using UnityEngine;

public class BloodVFXController : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private VisualEffect _VFX_Blood;

    [Header("Properties")]
    [SerializeField] private string _centerPropertyName;
    [SerializeField] private string _sizePropertyName;

    public void SpawnVFXBlood(Vector3 spawnPoint, Vector3 lookAt)
    {
        var direction = lookAt - _VFX_Blood.transform.position;
        direction.y = 0;

        var blood = Instantiate(_VFX_Blood, spawnPoint, Quaternion.LookRotation(direction));

        GetColliderToVFX(blood);
    }
    private void GetColliderToVFX(VisualEffect vfx)
    {
        float spawnHeight = vfx.transform.position.y;
        Vector3 worldSize = new Vector3();

        RaycastHit[] hits = Physics.RaycastAll(vfx.transform.position, Vector3.down);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag("Floor"))
            {
                spawnHeight = hit.distance;
                worldSize = Vector3.Scale(hit.collider.GetComponent<BoxCollider>().size, hit.collider.transform.lossyScale);
                break;
            }
        }

        Vector3 worldCenter = new Vector3(0, -(spawnHeight * 2 + worldSize.y / 2) + 0.05f, 0);

        vfx.SetVector3(_sizePropertyName, worldSize);
        vfx.SetVector3(_centerPropertyName, worldCenter);
    }
}
