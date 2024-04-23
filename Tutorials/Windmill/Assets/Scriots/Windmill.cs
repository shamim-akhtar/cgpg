using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Windmill : MonoBehaviour
{
    public int numBlades = 3;
    List<Transform> blades = new List<Transform>();
    public Transform rotor;

    // Start is called before the first frame update
    void Start()
    {
        float dangle = 360.0f / numBlades;
        List<Transform> blades = new List<Transform>();
        for (int i = 0; i < numBlades; i++)
        {
            // Create a new blade object
            GameObject bladeObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            bladeObject.transform.SetParent(rotor, false); // Set the rotor as parent

            // Position the blade around the center in a circular pattern
            float angle = i * dangle;

            bladeObject.transform.localScale = new Vector3(2.0f, 0.2f, 0.02f);

            //Vector3 rotationAngles = new Vector3(0.0f, 0.0f, angle);
            //bladeObject.transform.Rotate(rotationAngles);

            //Vector3 newPosition = new Vector3(0.0f, 0.0f, 0.1f);
            //bladeObject.transform.Translate(newPosition);

            // Add the blade to the list
            blades.Add(bladeObject.transform);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
