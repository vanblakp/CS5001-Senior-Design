using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [HideInInspector] public float moveSpeed;

    public GameObject bulletPrefab;
    public Rigidbody2D rb;
    public Animator animator;
    public Camera cam;
    public GameObject bulletSpawnPoint;

    public float bulletSpawnAdjust = 1.25f;
    public float fireRate = 1f;
    public float fireRateRandomness = 0.5f;
    public int delayToRemoveBullet = 8;

    private bool canShoot = true;
    private List<GameObject> firedBullets = new List<GameObject>();
    private bool isSprinting = false;
    public bool hasItem { get; set; } = false;
    public string pickedUpItem { get; set; } = "";

    Vector2 movement;
    Vector2 mousePos;

    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        moveSpeed = GetComponent<StaminaController>().walkingSpeed;
        //bulletSpawnPoint = this.gameObject.transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        // Take input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        
        // Fire bullet
        if (Input.GetButtonDown("Fire1") && canShoot)
        {
            StartCoroutine(FireRate());
        }

        // Retrieve mouse position on the screen
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

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
        bulletSpawnPoint.GetComponent<Rigidbody2D>().MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

        // Using mouse position on screen, set direction to fire bullet
        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        //rb.rotation = angle;
        bulletSpawnPoint.GetComponent<Rigidbody2D>().rotation = angle;
    }

    // Allows for adjusting the fire rate
    IEnumerator FireRate()
    {
        canShoot = false;

        GameObject firedBullet = Instantiate(bulletPrefab) as GameObject;
        firedBullet.layer = 9;
        firedBullet.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
        firedBullet.GetComponent<BulletController>().EnableDestroy(firedBullet, delayToRemoveBullet);
        firedBullet.transform.position = bulletSpawnPoint.transform.TransformPoint(Vector3.up * bulletSpawnAdjust);
        firedBullet.transform.rotation = bulletSpawnPoint.transform.rotation;
        firedBullets.Append(firedBullet);

        // Randomize the fire rate for more variation (player is set to 0)
        float actualRate = Random.Range(fireRate - fireRateRandomness, fireRate + fireRateRandomness);

        yield return new WaitForSeconds(actualRate);
        canShoot = true;
    }
}
