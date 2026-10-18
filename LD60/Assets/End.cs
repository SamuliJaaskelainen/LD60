using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class End : MonoBehaviour
{
    public GameObject pressSpace;

    public List<GameObject> fires;

    float timer = 0.0f;
    float fireTimer = 0.0f;

    void Start()
    {
        pressSpace.SetActive(false);
        Screenshake.Intensity = 0.15f;
    }

    void Update()
    {
        if(fireTimer < 2.5f)
        {
            fireTimer += Time.deltaTime;
            fires[0].SetActive(fireTimer < 1.0f);
            fires[1].SetActive(fireTimer < 1.5f);
            fires[2].SetActive(fireTimer < 2.0f);
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene("Menu");
            }

            if (Time.time > timer)
            {
                timer = Time.time + 0.33f;
                pressSpace.SetActive(!pressSpace.activeSelf);
                Screenshake.Intensity = 0.03f;
            }
        }
    }
}
