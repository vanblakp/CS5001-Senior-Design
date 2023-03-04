using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour
{
    public bool hasCannon = false;
    public int repairPercentage = 100;

    [SerializeField] private GameObject brokenChild;
    [SerializeField] private GameObject cannon;

    private new BoxCollider2D collider;
    private SpriteRenderer spriteRenderer;
    private HealthBase healthBase;

    void Start()
    {
        if (hasCannon)
        {
            cannon.gameObject.SetActive(hasCannon);
        }

        collider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        healthBase = GetComponent<HealthBase>();
    }

    void Update()
    {
        // If the wall is damaged, swap structure with broken rubble
        if (healthBase.currentHealth <= 0)
        {
            DestroyWall();
        }
        // If the wall is repaired enough, replace rubble with wall
        else if (healthBase.currentHealth >= (repairPercentage / 100) * healthBase.maxHealth)
        {
            RepairWall();
        }
    }

    // Destroys the wall and puts rubble in place
    void DestroyWall()
    {
        collider.enabled = false;
        spriteRenderer.enabled = false;
        cannon.SetActive(false);
        brokenChild.SetActive(true);
    }

    // Repairs the rubble into the wall
    void RepairWall()
    {
        brokenChild.SetActive(false);
        if (hasCannon)
        {
            cannon.SetActive(true);
        }
        collider.enabled = true;
        spriteRenderer.enabled = true;
    }
}
