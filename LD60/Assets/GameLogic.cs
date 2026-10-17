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
    public List<GameObject> campFires;
    public List<Transform> enemySpawners;
    public GameObject enemyPrefab;
    public float enemySpawnRate;
    float enemySpawnTimer;

    bool[] campFireStates = new bool[3];

    public IObjectPool<GameObject> enemyPool;
    public List<GameObject> enemies = new List<GameObject>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        for(int i = 0;  i < campFireStates.Length; i++)
        {
            campFireStates[i] = true;
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
    }

    void Update()
    {
        if(Time.time > enemySpawnTimer)
        {
            enemySpawnTimer = Time.time + enemySpawnRate;
            GameObject enemy = enemyPool.Get();
            enemy.transform.position = enemySpawners[Random.Range(0, enemySpawners.Count)].position;
        }
    }

    void UpdateCampfires()
    {
        int deadCampfires = 0;
        for (int i = 0; i < campFireStates.Length; i++)
        {
            if (campFireStates[i] == false) deadCampfires++;
            campFires[i].transform.Find("Fire").gameObject.SetActive(campFireStates[i]);
            uiOn[i].SetActive(campFireStates[i]);
            uiOff[i].SetActive(!campFireStates[i]);
        }

        if(deadCampfires == campFireAmount )
        {
            GameOver();
        }
    }

    void GameOver()
    {
        SceneManager.LoadScene(0);
    }

    public void SetCampfire(int i, bool isOn)
    {
        campFireStates[i] = isOn;
        UpdateCampfires();
    }

    public bool GetCampfireState(int i)
    {
        return campFireStates[i];
    }
}
