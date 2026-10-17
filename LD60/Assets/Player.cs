using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class Player : MonoBehaviour
{
    public static Player Instance;

    public Transform[] wheels;
    public Transform turret;
    public Transform turretTip;

    public float acceleration;
    public float drag;
    public float maxSpeed;
    public float maxReverseSpeed;
    public float turnSpeed;
    public float moveMaxDistance;
    public float wheelVisualMultiplier;
    public GameObject firePrefab;
    public float distanceToFire;
    public float fireRate;
    public float fireBurstRate;
    public int fireBurstAmount;
    public float maxFireSpread;

    float fireTimer;
    float currentSpeed;
    float currentTurnSpeed;
    Vector3 lastPosition;
    Vector3 velocity;

    public IObjectPool<GameObject> firePool;

    // TODO: Turboboost with space, drops fire

    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }

    public Vector3 GetVelocity()
    {
        return velocity;
    }

    void Awake()
    {
        Instance = this;

        firePool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(firePrefab),
            actionOnGet: (fire) => fire.gameObject.SetActive(true),
            actionOnRelease: (fire) => fire.gameObject.SetActive(false),
            actionOnDestroy: (fire) => Destroy(fire.gameObject),
            collectionCheck: true,
            defaultCapacity: 10,
            maxSize: 99
        );

        fireTimer = Time.time;
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

        // Bounce back to level
        if(Vector3.Distance(Vector3.zero, transform.position) > moveMaxDistance)
        {
            Vector3 directioToCenter = (Vector3.zero - transform.position).normalized;
            transform.Translate(directioToCenter * currentSpeed * Time.deltaTime, Space.World);
        }
        else
        {
            transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime, Space.Self);
        }

        // Spin wheels
        foreach(Transform w in wheels)
        {
            w.Rotate(Vector3.forward, currentSpeed * wheelVisualMultiplier, Space.Self);
        }

        // Turret lookat
        Transform closestTarget = null;
        float closestDistance = 99999.9f;
        
        foreach(GameObject e in GameLogic.Instance.enemies)
        {
            float distance = Vector3.Distance(transform.position, e.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = e.transform;
            }
        }

        for (int i = 0; i < GameLogic.Instance.campFireAmount; i++)
        {
            if(GameLogic.Instance.GetCampfireState(i) == false)
            { 
                float distance = Vector3.Distance(transform.position, GameLogic.Instance.campfires[i].transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = GameLogic.Instance.campfires[i].transform;
                }
            }
        }

        if (closestTarget != null)
        {
            if (closestDistance < distanceToFire)
            {
                turret.LookAt(closestTarget.position + Vector3.up * 1.0f, Vector3.up);

                // Shooting
                if (Time.time > fireTimer)
                {
                    fireTimer = Time.time + fireRate;
                    StartCoroutine("FireBurst");
                }
            }
            else
            {
                turret.LookAt(turret.position + transform.forward * 1.0f);
            }
        }

        // Velocity
        velocity = transform.position - lastPosition;
        lastPosition = transform.position;
        Debug.DrawRay(transform.position, velocity * 100.0f);
    }

    public void Fire()
    {
        GameObject fire = firePool.Get();
        fire.transform.position = turretTip.position;

        float xSpread = Random.Range(-maxFireSpread, maxFireSpread);
        float ySpread = Random.Range(-maxFireSpread, maxFireSpread);
        Vector3 spreadVector = new Vector3(xSpread, ySpread, 0.0f);
        spreadVector = transform.TransformVector(spreadVector);

        fire.transform.forward = (turretTip.forward + spreadVector).normalized;
    }

    IEnumerator FireBurst()
    {
        for(int i = 0; i < fireBurstAmount;  i++)
        {
            Fire();
            yield return new WaitForSeconds(fireBurstRate);
        }
    }
}
