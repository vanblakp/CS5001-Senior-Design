using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 100.0f;
    public int damage = 10;
    public const int delayToRemove = 8;

    // On spawn, launch projectile forwards and destory after specified amount of time
    private void Start()
    {
        gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * speed);
        //Destroy(gameObject, delayToRemove);
    }

    public void EnableDestroy(GameObject obj, int delay=delayToRemove)
    {
        Destroy(obj, delay);
    }

    // What to do when bullet collides
    void OnTriggerEnter2D(Collider2D collision)
    {
        // When bullet hits the player, an enemy, or a wall, damage the associated actor
        if ((collision.isTrigger && collision.tag == "Player") || (!collision.isTrigger && (collision.tag == "Player" || collision.tag == "Enemy") || collision.tag == "Wall"))
        {
            HealthBase actorHealth = collision.GetComponent<HealthBase>();
            if (actorHealth != null)
            {
                actorHealth.DamageActor(damage);
            }
            Destroy(this.gameObject);
        }
        

        // Could spawn an effect/animation (i.e. when a bullet hits a wall, or when it hits player/enemy)
    }
}
