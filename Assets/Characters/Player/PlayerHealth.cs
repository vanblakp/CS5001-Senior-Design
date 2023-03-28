using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHealth : HealthBase
{
    public ProgressBar healthBar;

    private new void Start()
    {
        base.Start();
        healthBar.SetMaxValue(maxHealth);
    }

    public override void DamageActor(int damage)
    {
        base.DamageActor(damage);
        healthBar.SetValue(currentHealth);
    }

    public override void HealActor(int healing)
    {
        base.HealActor(healing);
        healthBar.SetValue(currentHealth);
    }
}
