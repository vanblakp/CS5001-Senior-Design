using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBase : MonoBehaviour
{
    [HideInInspector] public int currentHealth;
    [HideInInspector] public bool isAlive = true;
    public int maxHealth;
    public bool removeWhenZero = true;

    // Start is called before the first frame update
    public void Start()
    {
        currentHealth = maxHealth;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Damages the current actor with given amount of damage
    public virtual void DamageActor(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0 && removeWhenZero)
        {
            isAlive = false;
            Animator animator = gameObject.GetComponent<Animator>();
            animator.Play("Death");
            Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length); 
            // Then reload the level or bring up menu
        }
    }

    // Heals the current actor with given amount of health
    public virtual void HealActor(int health)
    {
        currentHealth += health;
        if (currentHealth > 100)
        {
            currentHealth = 100;
        }
    }
}
