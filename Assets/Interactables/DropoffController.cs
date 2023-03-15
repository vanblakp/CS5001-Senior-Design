using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DropoffController : MonoBehaviour
{
    private bool canCheck = false;
    private PlayerMovement playerMovement;
    private HealthBase healthBase;

    // Start is called before the first frame update
    void Start()
    {
        healthBase = this.transform.parent.gameObject.GetComponentInParent<HealthBase>();
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

                playerMovement.hasItem = false;
                playerMovement.pickedUpItem = "";

                // Repair health based on a percentage of the maximum health + 1 for adjustment
                healthBase.currentHealth = healthBase.currentHealth + ((healthBase.maxHealth / 3) + 1);
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
