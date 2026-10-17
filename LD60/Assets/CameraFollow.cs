using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Vector3 offset;
    public float speedOffsetMultiplier;
    public float minCameraSize;
    public float cameraSizeMultiplier;
    public float cameraSizeChangeSpeedUp;
    public float cameraSizeChangeSpeedDown;
    public float camereSizeChangeDownDelay;
    float cameraSizeTarget;
    Camera mainCamera;
    float camereSizeChangeDownTimer;

    void Start()
    {
        mainCamera = GetComponentInChildren<Camera>();
        mainCamera.orthographicSize = minCameraSize;
    }


    void Update()
    {
        transform.position = Player.Instance.transform.position + offset + Player.Instance.transform.forward * Player.Instance.GetCurrentSpeed() * speedOffsetMultiplier;

        cameraSizeTarget = Mathf.Abs(Player.Instance.GetCurrentSpeed()) * cameraSizeMultiplier;
        cameraSizeTarget = Mathf.Max(cameraSizeTarget, minCameraSize);

        if(mainCamera.orthographicSize < cameraSizeTarget)
        {
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, cameraSizeTarget, cameraSizeChangeSpeedUp * Time.deltaTime);
            camereSizeChangeDownTimer = 0.0f;
        }
        else
        {
            if(camereSizeChangeDownTimer > camereSizeChangeDownDelay)
            { 
                mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, cameraSizeTarget, cameraSizeChangeSpeedUp * Time.deltaTime);
            }
            else
            {
                camereSizeChangeDownTimer += Time.deltaTime;
            }
        }
    }
}
