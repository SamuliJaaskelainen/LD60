using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour
{
    public static GameLogic Instance;

    public int campFireAmount = 3;

    public GameObject[] uiOn;
    public GameObject[] uiOff;
    public List<GameObject> campfires;
    public List<Transform> enemySpawners;
    public GameObject enemyPrefab;
    public float enemySpawnRate;
    float enemySpawnTimer;

    bool[] campfireStates = new bool[3];

    public IObjectPool<GameObject> enemyPool;
    public List<GameObject> enemies = new List<GameObject>();

    void Awake()
    {
        Instance = this;

        for(int i = 0;  i < campfireStates.Length; i++)
        {
            campfireStates[i] = true;
            campfires[i].GetComponent<Campfire>().index = i;
        }

        UpdateCampfires();

        enemyPool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(enemyPrefab),
            actionOnGet: (enemy) => { enemy.gameObject.SetActive(true); enemies.Add(enemy); },
            actionOnRelease: (enemy) => { enemy.gameObject.SetActive(false); enemies.Remove(enemy); },
            actionOnDestroy: (enemy) => { Destroy(enemy.gameObject); enemies.Remove(enemy); },
            collectionCheck: true,
            defaultCapacity: 10,
            maxSize: 99
        );

        AudioManager.Instance.SetSilence(2.4f);
    }

    void Update()
    {
        if(Time.time > enemySpawnTimer)
        {
            enemySpawnTimer = Time.time + enemySpawnRate;
            GameObject enemy = enemyPool.Get();

            Vector3 randomSpawnCircle = new Vector3(Random.insideUnitCircle.x * 3.0f, 0.0f, Random.insideUnitCircle.x * 3.0f);
            enemy.transform.position = enemySpawners[Random.Range(0, enemySpawners.Count)].position + randomSpawnCircle;
        }
    }

    void UpdateCampfires()
    {
        int deadCampfires = 0;
        for (int i = 0; i < campfireStates.Length; i++)
        {
            if (campfireStates[i] == false) deadCampfires++;
            campfires[i].transform.Find("Fire").gameObject.SetActive(campfireStates[i]);
            uiOn[i].SetActive(campfireStates[i]);
            uiOff[i].SetActive(!campfireStates[i]);
        }

        if(deadCampfires == campFireAmount )
        {
            GameOver();
        }
    }

    void GameOver()
    {
        SceneManager.LoadScene("End");
    }

    public void SetCampfire(int i, bool isOn)
    {
        if(!isOn)
        {
            Screenshake.Intensity = 0.1f;
        }

        campfireStates[i] = isOn;
        UpdateCampfires();
    }

    public bool GetCampfireState(int i)
    {
        return campfireStates[i];
    }
}
