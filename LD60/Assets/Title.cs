using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public GameObject pressSpace;

    float timer;
    float startTimer;

    void Update()
    {
        if(startTimer < 2.0f)
        {
            startTimer += Time.deltaTime;
        }

        if(Input.GetKeyDown(KeyCode.Space) && startTimer > 1.0f)
        {
            SceneManager.LoadScene("Main");
        }

        if(Time.time > timer)
        {
            timer = Time.time + 0.33f;
            pressSpace.SetActive(!pressSpace.activeSelf);
        }
    }
}
