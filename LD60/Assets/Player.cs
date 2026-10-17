using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;

    public Transform[] wheels;

    public float acceleration;
    public float drag;
    public float maxSpeed;
    public float maxReverseSpeed;
    public float turnSpeed;
    public float wheelVisualMultiplier;
    float currentSpeed;
    float currentTurnSpeed;

    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        // Turn
        float absSpeed = Mathf.Abs(currentSpeed);
        currentTurnSpeed = Mathf.Lerp(0, maxSpeed, absSpeed) / maxSpeed * turnSpeed;
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(Vector3.up, currentTurnSpeed * Time.deltaTime, Space.Self);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(Vector3.up, -currentTurnSpeed * Time.deltaTime, Space.Self);
        }

        // Accelerate
        if (Input.GetKey(KeyCode.UpArrow))
        {
            currentSpeed += acceleration * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            currentSpeed -= acceleration * Time.deltaTime;
        }
        else
        {
            if(currentSpeed > 0.0f)
            {
                currentSpeed -= drag * Time.deltaTime;
            }
            else
            {
                currentSpeed += drag * Time.deltaTime;
            }
           
        }
        currentSpeed = Mathf.Clamp(currentSpeed, maxReverseSpeed, maxSpeed);
        transform.Translate(Vector3.forward * currentSpeed, Space.Self);

        // Spin wheels
        foreach(Transform w in wheels)
        {
            w.Rotate(Vector3.forward, currentSpeed * wheelVisualMultiplier, Space.Self);
        }
    }
}
