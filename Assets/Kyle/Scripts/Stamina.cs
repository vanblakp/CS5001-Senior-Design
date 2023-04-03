using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stamina : Collectable
{
    public Sprite emptyStamina;
    public int stamina = 10;
    public float soundDelay = 0.1f;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.isTrigger && collision.tag == "Player"))
        {
            // Add stamina if not at full stamina
            StaminaController stamina = collision.GetComponent<StaminaController>();
            
            stamina.AddStamina(100);

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
