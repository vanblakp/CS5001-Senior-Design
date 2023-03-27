using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaController : MonoBehaviour
{
    [Header("Stamina Main Parameters")]
    [Range(0.0f, 200.0f)] private float playerStamina = 100.0f;
    [SerializeField] private float maxStamina = 100.0f;
    [HideInInspector] public bool hasRegenerated = true;
    [HideInInspector] public bool isSprinting = false;

    [Header("Stamina Regen Parameters")]
    [Range(0, 50)][SerializeField] private float staminaDrain = 20.0f;
    [Range(0, 50)][SerializeField] private float staminaRegen = 8f;
    [Range(0, 10)][SerializeField] private float regenDelay = 3;
    private bool isRegenerating = false;
    private bool delayingRegen = false;

    [Header("Stamina Speed Parameters")]
    [SerializeField] public int sprintingSpeed = 9;
    [SerializeField] public int walkingSpeed = 6;
    [SerializeField] public int slowedSpeed = 3;

    [Header("Stamina UI Elements")]
    public ProgressBar staminaBar;

    private void Start()
    {
        staminaBar.SetMaxValue(maxStamina);
        playerStamina = maxStamina;
    }

    // Update is called once per frame
    void Update()
    {
        // Sets to true if the player begins sprinting while walking
        isSprinting = Input.GetButton("Sprint") && (Input.GetButton("Horizontal") || Input.GetButton("Vertical")) && playerStamina > 0;

        // If player is not sprinting (or using their stamina)
        if (!isSprinting)
        {
            // If the player was not exhausted from removing all stamina, keep walking speed
            if (playerStamina > 0)
            {
                // Set to normal speed
                GetComponent<PlayerMovement>().moveSpeed = walkingSpeed;
            }

            // While the current stamina is less than the maximum stamina, regen stamina
            if (playerStamina < maxStamina)
            {
                if (!delayingRegen && !isRegenerating)
                {
                    //print("STARTING COROUTINE");
                    StartCoroutine(RegenStaminaDelay());
                }
                else if (isRegenerating)
                {
                    playerStamina += staminaRegen * Time.deltaTime;
                    staminaBar.SetValue(playerStamina);
                    //print("REGEN: " + playerStamina);
                }

                // If the current stamina reaches maximum, stop regen
                if (playerStamina >= maxStamina)
                {
                    // Reset stamina bar alpha value
                    //sliderCanvasGroup.alpha = 0;

                    hasRegenerated = true;
                    isRegenerating = false;
                }
            }
            
        }
        else
        {
            // Stop the stamina regen delay so it doesn't carry over
            StopCoroutine(RegenStaminaDelay());

            Sprinting();
        }
    }

    // Function to delay the regeneration of stamina for a few seconds
    private IEnumerator RegenStaminaDelay()
    {
        delayingRegen = true;

        //print("DELAYING: " + Time.time);
        yield return new WaitForSeconds(regenDelay);

        delayingRegen = false;

        //print("STARTING REGEN: " + Time.time);
        // If player is sprinting, cancel stamina regen
        if (isSprinting)
        {
            //print("CANCELED REGEN DUE TO SPRINT");
            isRegenerating = false;
        }
        isRegenerating = true;
        yield break;
    }

    // Drains stamina over time due to player sprinting
    public void Sprinting()
    {
        hasRegenerated = false;
        isRegenerating = false;

        // If stamina exists, speed player up and drain stamina
        if (playerStamina > 0)
        {
            // Set to sprinting speed
            GetComponent<PlayerMovement>().moveSpeed = sprintingSpeed;
            playerStamina -= staminaDrain * Time.deltaTime;
            staminaBar.SetValue(playerStamina);
            //print("DRAIN: " + playerStamina);

            // Once stamina does not exist, stop sprinting and slow the player down
            if (playerStamina <= 0)
            {
                // Slow the player after running out of stamina
                GetComponent<PlayerMovement>().moveSpeed = slowedSpeed;
                //print("EXHAUSTED");

                // Reset stamina bar alpha value
                //sliderCanvasGroup.alpha = 0;
            }

            //UpdateStamina(1);
        }
    }
}
