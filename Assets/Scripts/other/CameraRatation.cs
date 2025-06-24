using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRatation : MonoBehaviour
{
    public float MinAngel;
    public float MaxAngel;

    public Transform CameraAxisTransform;

    public float RotatinSpeedY;
    public float RotatinSpeedX;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y + Time.deltaTime * RotatinSpeedX * Input.GetAxis("Mouse X"), 0);

        var newAngelX = CameraAxisTransform.localEulerAngles.x - Time.deltaTime * RotatinSpeedY * Input.GetAxis("Mouse Y");
        if (newAngelX > 180)
            newAngelX -= 360;
        newAngelX = Mathf.Clamp(newAngelX, MinAngel, MaxAngel);

        CameraAxisTransform.localEulerAngles = new Vector3(newAngelX, 0, 0);
    }
}
