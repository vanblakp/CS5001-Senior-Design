using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PickupController : MonoBehaviour
{
    public string item = "Brick";
    public Sprite spriteImg;

    public float soundDelay = 0.5f;
    public float volumeChangeMultiplier = 0.2f;
    public float pitchChangeMultiplier = 0.2f;

    private GameObject itemUISpot;

    private bool canCheck = false;
    private PlayerMovement playerMovement;

    private void Start()
    {
        itemUISpot = GameObject.Find("HUDCanvas").transform.Find("Item").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (canCheck)
        {
            // If the player presses E
            if (Input.GetKeyDown(KeyCode.E) && !playerMovement.hasItem)
            {
                // Play sound effect if one exists
                if (gameObject.TryGetComponent(out AudioSource audioSource))
                {
                    audioSource.time = soundDelay;
                    audioSource.volume = Random.Range(1 - volumeChangeMultiplier, 1);
                    audioSource.pitch = Random.Range(1 - pitchChangeMultiplier, 1 + pitchChangeMultiplier);
                    audioSource.Play(0);
                }

                playerMovement.hasItem = true;
                playerMovement.pickedUpItem = item;

                // Add item to HUD UI
                Image image = itemUISpot.GetComponent<Image>();

                image.enabled = true;
                image.sprite = spriteImg;

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
