using AudioRender;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenshake : MonoBehaviour
{
    public static float Intensity = 0.0f;
    public float speed = 1.0f;

    WireframeRenderer wireRenderer;

    void Start()
    {
        wireRenderer = GetComponent<WireframeRenderer>();
    }

    void Update()
    {
        if(Intensity > 0.0f)
        {
            Intensity -= speed * Time.deltaTime;
        }
        else
        {
            Intensity  = 0.0f;
        }

        wireRenderer.randomOffset = Intensity;
    }
}
