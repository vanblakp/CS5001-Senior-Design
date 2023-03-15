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
        //gameObject.name = "Bullet " + Random.RandomRange(0,10000);
        //Destroy(gameObject, delayToRemove);
    }

    public void EnableDestroy(GameObject obj, int delay=delayToRemove)
    {
        Destroy(obj, delay);
    }

    // What to do when bullet collides
    void OnTriggerEnter2D(Collider2D collision)
    {
        bool canDamage = false;
        // Collision layer checks to determine what bullet hit and if it should do anything
        // When the bullet hits an enemy and is not the enemy range trigger
        if (collision.gameObject.layer == 7 && !collision.isTrigger)
        {
            // When this is an enemy bullet
            if (this.gameObject.layer == 6)
            {
                print("ENEMY HIT ENEMY, IGNORE");
                canDamage = false;
                return;
            }
            // When this is a player bullet
            else if (this.gameObject.layer == 9)
            {
                print("PLAYER HIT ENEMY");
                canDamage = true;
            }
        }
        // When the bullet hits a player
        else if (collision.gameObject.layer == 8)
        {
            // When this is an enemy bullet
            if (this.gameObject.layer == 6)
            {
                print("ENEMY HIT PLAYER");
                canDamage = true;
            }
            // When this is a player bullet
            else if (this.gameObject.layer == 9)
            {
                print("PLAYER HIT PLAYER, IGNORE");
                canDamage = false;
                return;
            }
        }

        // When bullet hits the player, an enemy, or a wall, damage the associated actor
        if (!collision.isTrigger && ((canDamage && collision.tag == "Player" || collision.tag == "Enemy") || collision.tag == "Wall"))
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
