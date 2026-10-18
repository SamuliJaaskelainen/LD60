using UnityEngine;

public class Fire : MonoBehaviour
{
    public AnimationCurve scaleAnim;
    public float speed;
    public float gravity;
    public float lifetime;
    public float groundSlowdownMultiplier;
    float currentLifetime;
    float currentSpeed;
    Vector3 inheritedVelocity;

    void OnEnable()
    {
        currentLifetime = lifetime;
        currentSpeed = speed;

        Vector3 playerVelocity = Player.Instance.GetVelocity().normalized * Player.Instance.GetCurrentSpeed();
        float velocityMultipier = Vector3.Dot(Player.Instance.transform.forward, Player.Instance.turret.forward);
        velocityMultipier = Mathf.Abs(velocityMultipier);
        inheritedVelocity = playerVelocity * velocityMultipier;
        transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, scaleAnim.Evaluate(currentLifetime / lifetime));
    }

    void Update()
    {
        Vector3 movement = transform.forward * currentSpeed;
        movement.y -= gravity;
        movement += inheritedVelocity;
        if (transform.position.y < 0.0f)
        {
            movement.y = 0.0f;
            transform.Translate(movement * groundSlowdownMultiplier * Time.deltaTime, Space.World);
        }
        else
        {
            transform.Translate(movement * Time.deltaTime, Space.World);
        }

        currentLifetime -= Time.deltaTime;
        transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, scaleAnim.Evaluate(currentLifetime / lifetime));

        if(currentLifetime < 0.0f)
        {
            Player.Instance.firePool.Release(gameObject);
        }
    }

    public void OnTriggerEnter(Collider c)
    {
        Enemy enemy = c.transform.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.Die(true);
            return;
        }

        Campfire f = c.transform.GetComponent<Campfire>();
        if (f != null)
        {
            if(!GameLogic.Instance.GetCampfireState(f.index))
            { 
                AudioManager.Instance.ForceNextSound();
                AudioManager.Instance.PlaySound("Lit");
                AudioManager.Instance.SetSilence(1.7f);
                GameLogic.Instance.SetCampfire(f.index, true);
            }
        }
    }
}
