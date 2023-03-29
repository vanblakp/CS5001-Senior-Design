using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DropoffController : MonoBehaviour
{
    private bool canCheck = false;
    private PlayerMovement playerMovement;
    private HealthBase healthBase;

    public int repairNeeded = 3;

    public float soundDelay = 0.5f;
    public float volumeChangeMultiplier = 0.2f;
    public float pitchChangeMultiplier = 0.2f;

    private GameObject itemUISpot;

    // Start is called before the first frame update
    void Start()
    {
        healthBase = this.transform.parent.gameObject.GetComponentInParent<HealthBase>();
        itemUISpot = GameObject.Find("HUDCanvas").transform.Find("Item").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (canCheck)
        {
            // If the player presses E
            if (Input.GetKeyDown(KeyCode.E) && playerMovement.hasItem && !(healthBase.currentHealth >= 100))
            {
                print("Dropped off " + playerMovement.pickedUpItem);

                // Play sound effect if one exists
                if (gameObject.transform.parent.transform.parent.GetComponent<AudioSource>())
                {
                    AudioSource audioSource = gameObject.transform.parent.transform.parent.GetComponent<AudioSource>();
                    audioSource.time = soundDelay;
                    audioSource.volume = Random.Range(1 - volumeChangeMultiplier, 1);
                    audioSource.pitch = Random.Range(1 - pitchChangeMultiplier, 1 + pitchChangeMultiplier);
                    audioSource.Play(0);
                }

                playerMovement.hasItem = false;
                playerMovement.pickedUpItem = "";

                // Remove item from HUD UI
                Image image = itemUISpot.GetComponent<Image>();

                image.enabled = false;
                image.sprite = null;

                // Repair health based on a percentage of the maximum health + 1 for adjustment
                healthBase.currentHealth = healthBase.currentHealth + ((healthBase.maxHealth / repairNeeded) + 1);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If the player is in range, allow pickup
        if (collision.tag == "Player" && collision.gameObject.layer == 8 && collision.isTrigger)
        {
            playerMovement = collision.gameObject.GetComponent<PlayerMovement>();
            canCheck = true;
        }
        else
        {
            canCheck = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // If the player leaves range, remove pickup
        if (collision.tag == "Player" && collision.gameObject.layer == 8 && collision.isTrigger)
        {
            canCheck = false;
        }
    }
}
