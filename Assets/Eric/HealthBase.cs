using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBase : MonoBehaviour
{
    [HideInInspector] public int currentHealth;
    public int maxHealth;
    public bool removeWhenZero = true;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Damages the current actor with given amount of damage
    public void DamageActor(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0 && removeWhenZero)
        {
            gameObject.SetActive(false);
            // Then reload the level or bring up menu
        }
    }
}
