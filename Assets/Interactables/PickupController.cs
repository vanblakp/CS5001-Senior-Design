using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    public string item = "Brick";

    private bool canCheck = false;
    private PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canCheck)
        {
            // If the player presses E
            if (Input.GetKeyDown(KeyCode.E) && !playerMovement.hasItem)
            {
                playerMovement.hasItem = true;
                playerMovement.pickedUpItem = item;

                print("Picked up " + item);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If the player is in range, allow pickup
        if (collision.tag == "Player" && collision.gameObject.layer == 8 && collision.isTrigger)
        {
            playerMovement =  collision.gameObject.GetComponent<PlayerMovement>();
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
