using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public float angleX = 30.0f;
    void Start()
    {
        // Translate the GameObject along the forward direction
        transform.Translate(0.0f, 0.0f, 0.0f);

        // Apply Euler rotation to the GameObject
        // The angle is specified in degrees, so we convert it to radians
        float rotationAngle = angleX * Mathf.Deg2Rad;
        transform.Rotate(angleX, 0.0f, 0.0f);
    }
}
