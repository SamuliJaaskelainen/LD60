using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public GameObject pressSpace;

    float timer;

    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
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
