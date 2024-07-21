using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS : MonoBehaviour
{
    public float speed = 5.0f;
    public float lookSensitivity = 2.0f;
    public float maxLookX = 60.0f;
    public float minLookX = -60.0f;

    private float rotX;
    public Camera cam;

    void Start()
    {
        // Lock the cursor
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Move();
        CamLook();
    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float z = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        Vector3 move = transform.right * x + transform.forward * z;
        transform.position += move;
    }

    void CamLook()
    {
        float y = Input.GetAxis("Mouse X") * lookSensitivity;
        rotX -= Input.GetAxis("Mouse Y") * lookSensitivity;

        rotX = Mathf.Clamp(rotX, minLookX, maxLookX);

        cam.transform.localRotation = Quaternion.Euler(rotX, 0, 0);
        transform.eulerAngles += Vector3.up * y;
    }
}
