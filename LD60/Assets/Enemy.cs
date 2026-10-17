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

    public static List<Enemy> enemies = new List<Enemy>();

    void Awake()
    {
        startScale = transform.localScale;
        agent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        enemies.Add(this);
    }

    private void OnDisable()
    {
        enemies.Remove(this);
    }

    void Update()
    {
        // Move
        agent.SetDestination(Player.Instance.transform.position);

        // Animate
        float a = anim.Evaluate(animTime) * animMultipier;
        animTime += animSpeed * Time.deltaTime;
        if(animTime > 1.0f) animTime -= 1.0f;
        transform.localScale = new Vector3(startScale.x, startScale.y * a, startScale.z);
    }
}
