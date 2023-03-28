using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Collectable
{
    public Sprite emptyChest;
    public int health = 10;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.isTrigger && collision.tag == "Player"))
        {
            HealthBase actorHealth = collision.GetComponent<HealthBase>();
            if (actorHealth != null)
            {
                actorHealth.HealActor(health);
            }
            Destroy(this.gameObject);
        }

    }
}
