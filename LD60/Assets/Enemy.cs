using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public AnimationCurve anim;
    public float animMultipier;
    public float animSpeed;
    Vector3 startScale;
    float animTime;
    NavMeshAgent agent;
    int targetFire;

    void Awake()
    {
        startScale = transform.localScale;
        agent = GetComponent<NavMeshAgent>();
    }

    void OnEnable()
    {
        InitNewTarget();
    }

    void InitNewTarget()
    {
        targetFire = Random.Range(0, GameLogic.Instance.campFireAmount);
        agent.SetDestination(GameLogic.Instance.campfires[targetFire].transform.position);
    }

    void Update()
    {
        // Animate
        float a = anim.Evaluate(animTime) * animMultipier;
        animTime += animSpeed * Time.deltaTime;
        if(animTime > 1.0f) animTime -= 1.0f;
        transform.localScale = new Vector3(startScale.x, startScale.y * a, startScale.z);

        // Kill fire
        if(Vector3.Distance(transform.position, agent.destination) < 0.5f)
        {
            if(GameLogic.Instance.GetCampfireState(targetFire))
            {
                AudioManager.Instance.ForceNextSound();
                AudioManager.Instance.PlaySound("Danger");
                AudioManager.Instance.SetSilence(2.3f);
                GameLogic.Instance.SetCampfire(targetFire, false);
                Die();
            }
            else
            {
                InitNewTarget();
            }
        }
    }

    public void Die(bool playAudio = false)
    {
        if(playAudio)
        {
            AudioManager.Instance.PlayRandomSound("H", 3);
        }

        GameLogic.Instance.enemyPool.Release(gameObject);
    }
}
