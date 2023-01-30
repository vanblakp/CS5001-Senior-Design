using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [HideInInspector] public float moveSpeed;

    public Rigidbody2D rb;
    public Animator animator;

    private bool isSprinting = false;

    Vector2 movement;

    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        moveSpeed = GetComponent<StaminaController>().walkingSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        // Take input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Normalize to avoid diagonal speed being faster
        movement = movement.normalized;

        // Adjust parameters in animator component
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        // Flips the side animation when player goes left since only the right one is currently used
        spriteRenderer.flipX = movement.x < 0.01 ? true : false;

        // Sets to true if the player begins sprinting while walking
        isSprinting = Input.GetButton("Sprint") && (Input.GetButton("Horizontal") || Input.GetButton("Vertical"));
    }

    // Similar to Update except it's consistent to avoid issues in changing framerates
    private void FixedUpdate()
    {
        // Move player
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
