using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 100.0f;
    public int damage = 10;
    public int delayToRemove = 20;

    // On spawn, launch projectile forwards and destory after specified amount of time
    private void Start()
    {
        gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * speed);
        Destroy(gameObject, delayToRemove);
    }

    // What to do when bullet collides
    void OnTriggerEnter2D(Collider2D collision)
    {
        // When bullet hits the player, an enemy, or a wall, damage the associated actor
        if (collision.tag == "Player" || collision.tag == "Enemy" || collision.tag == "Wall")
        {
            HealthBase actorHealth = collision.GetComponent<HealthBase>();
            if (actorHealth != null)
            {
                actorHealth.DamageActor(damage);
            }
        }
        Destroy(this.gameObject);

        // Could spawn an effect/animation (i.e. when a bullet hits a wall, or when it hits player/enemy)
    }
}
