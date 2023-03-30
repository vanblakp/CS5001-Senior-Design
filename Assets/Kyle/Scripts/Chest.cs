using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Collectable
{
    public Sprite emptyChest;
    public int health = 10;
    public float soundDelay = 0.1f;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.isTrigger && collision.tag == "Player"))
        {
            // Heal player if not at full health
            HealthBase actorHealth = collision.GetComponent<HealthBase>();
            if (actorHealth != null && actorHealth.currentHealth < actorHealth.maxHealth)
            {
                actorHealth.HealActor(health);

                // Play sound effect if one exists
                if (gameObject.transform.parent.GetComponent<AudioSource>())
                {
                    AudioSource audioSource = gameObject.transform.parent.GetComponent<AudioSource>();
                    audioSource.time = soundDelay;
                    audioSource.Play(0);
                }
                Destroy(this.gameObject);
            }
        }

    }
}
