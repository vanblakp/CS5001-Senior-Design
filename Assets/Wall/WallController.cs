using NavMeshPlus.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour
{
    public bool hasCannon = false;
    public int repairPercentage = 100;
    public AudioClip destroySound;

    [SerializeField] private GameObject brokenChild;
    [SerializeField] private GameObject cannon;

    private BoxCollider2D collider;
    private SpriteRenderer spriteRenderer;
    private HealthBase healthBase;
    private bool repaired = true;
    //private NavMeshModifier navMeshModifier;

    void Start()
    {
        if (hasCannon)
        {
            cannon.gameObject.SetActive(hasCannon);
        }

        collider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        healthBase = GetComponent<HealthBase>();
        //navMeshModifier = GetComponent<NavMeshModifier>();
    }

    void Update()
    {
        // If the wall is damaged, swap structure with broken rubble
        if (healthBase.currentHealth <= 0 && repaired)
        {
            DestroyWall();
        }
        // If the wall is repaired enough (i.e. 100% of max health needed to repair), replace rubble with wall
        else if (healthBase.currentHealth >= (repairPercentage / 100) * healthBase.maxHealth && !repaired)
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
        repaired = false;

        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.clip = destroySound;
        audioSource.Play();
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
        repaired = true;
    }

    public bool getIsRepaired()
    {
        return repaired;
    }
}
