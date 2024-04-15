using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Fan : MonoBehaviour
{
    public Transform rotor;
    public int numBlades = 3;
    List<Transform> blades = new List<Transform>();

    float rpm = 0f;

    // Start is called before the first frame update
    void Start()
    {
        float dangle = 360.0f / numBlades;
        for (int i = 0; i < numBlades; i++)
        {
            // Create a new blade object
            GameObject bladeObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            bladeObject.transform.SetParent(rotor, false); // Set the rotor as parent

            // Position the blade around the center in a circular pattern
            float angle = i * dangle;

            bladeObject.transform.localScale = new Vector3(0.5f, 0.1f, 0.02f);

            Vector3 rotationAngles = new Vector3(0.0f, 0.0f, angle);
            bladeObject.transform.Rotate(rotationAngles);

            Vector3 newPosition = new Vector3(0.4f, 0.0f, 0.1f);
            bladeObject.transform.Translate(newPosition);

            // Add the blade to the list
            blades.Add(bladeObject.transform);
        }
    }

    public void Rotate()
    {
        // Calculate the angle to rotate per frame based on RPM and deltaTime
        float degreesPerSecond = rpm * 360.0f / 60.0f; // Convert RPM to degrees per second
        float degreesPerFrame = degreesPerSecond * Time.deltaTime;

        // Rotate the rotor
        rotor.Rotate(rotor.forward, degreesPerFrame);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            rpm = 10.0f;
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            rpm = 30.0f;
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            rpm = 50.0f;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            rpm = 0.0f;
        }
        Rotate();
    }
}
