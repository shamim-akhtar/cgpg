using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pixel : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetColor(Color col)
    {
        spriteRenderer.color = col;
    }
}
