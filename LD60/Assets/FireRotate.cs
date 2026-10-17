using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRotate : MonoBehaviour
{
    public Vector3 axis;
    public float degreesPerSecondMin;
    public float degreesPerSecondMax;
    public float rate = 60.0f;
    float timer = 0.0f;
    
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > rate)
        {
            timer = 0.0f;
            float r = Random.Range(degreesPerSecondMin, degreesPerSecondMax);
            transform.Rotate(axis, r);
        }
    }
}
