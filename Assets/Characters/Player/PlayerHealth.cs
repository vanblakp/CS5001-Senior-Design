using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHealth : HealthBase
{
    public ProgressBar healthBar;

    private bool alive = true;

    private new void Start()
    {
        base.Start();
        healthBar.SetMaxValue(maxHealth);
    }

    public override void DamageActor(int damage)
    {
        base.DamageActor(damage);
        healthBar.SetValue(currentHealth);

        // If player has died, display death UI
        if (currentHealth <= 0 && removeWhenZero && alive)
        {
            alive = false;
            PlayerDied();
        }
    }

    public override void HealActor(int healing)
    {
        base.HealActor(healing);
        healthBar.SetValue(currentHealth);
    }

    private void PlayerDied()
    {
        LevelManager.instance.GameOver();
    }
}
