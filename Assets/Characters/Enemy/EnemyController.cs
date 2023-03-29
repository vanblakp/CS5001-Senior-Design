using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    [Header("Bullet Parameters")]
    public GameObject bulletPrefab;
    public float bulletSpawnAdjust = 1.25f;
    public int range = 20;
    public float fireRate = 1f;
    public float fireRateRandomness = 0.5f;
    public int delayToRemoveBullet = 8;
    public int bulletDamage = 10;
    public AudioClip[] gunshotClips;
    public float volumeChangeMultiplier = 0.2f;
    public float pitchChangeMultiplier = 0.2f;

    [Header("Movement Parameters")]
    public int maxDistanceToObject = 10;
    public float moveSpeed = 1;

    private AudioSource gunshotSound;

    [Header("Other Parameters")]
    [SerializeField] private GameObject player;
    private GameObject bulletSpawnPoint;

    private Rigidbody2D rb;
    private DetectionZone detectionZone;
    private List<GameObject> firedBullets = new List<GameObject>();
    private bool canShoot = true;

    private Transform target;
    private NavMeshAgent agent;

    private bool playerFound = false;
    private bool brokenWallFound = false;
    private bool insideWalls = false;

    private HealthBase healthBase;

    private Animator animator;
    bool facingRight;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        detectionZone = GetComponent<DetectionZone>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        bulletSpawnPoint = transform.Find("BulletSpawnPoint").gameObject;
        healthBase = gameObject.GetComponent<HealthBase>();
        animator = GetComponent<Animator>();
        facingRight = true;

        gunshotSound = GetComponent<AudioSource>();
        gunshotSound.clip = gunshotClips[Random.Range(0, gunshotClips.Length)];
    }

    private void FixedUpdate()  // Works aside from a few cases where an enemy breaks a wall, and while moving towards the broken wall they break another
                                // which causes them to get stuck bouncing back and forth between the two while shooting at more walls
    {
        if (healthBase.isAlive)
        {
            if (detectionZone.detectedObjs.Count > 0)
            {
                int i = 0;
                float closest = -1;
                Collider2D moveToObj = null;
                playerFound = false;
                brokenWallFound = false;
                // Loop through all the detected objects in enemy range
                foreach (Collider2D detectedObj in detectionZone.detectedObjs)
                {
                    // If not null (if player hasn't died and collision is removed)
                    if (detectedObj)
                    {
                        // Check if player is in range and enemy is within the walls
                        if (detectionZone.detectedObjs[i].tag == "Player" && insideWalls)
                        {
                            playerFound = true;
                            moveToObj = detectionZone.detectedObjs[i];
                        }
                        // Check if there is a broken wall nearby and enemy is outside walls
                        else if (detectionZone.detectedObjs[i].tag == "Wall" && !detectionZone.detectedObjs[i].GetComponentInParent<WallController>().getIsRepaired() && !insideWalls && !brokenWallFound)
                        {
                            brokenWallFound = true;
                            moveToObj = detectionZone.detectedObjs[i];
                        }

                        // Find the closest object if player isn't in range and target it
                        float distance = Vector3.Distance(transform.position, detectionZone.detectedObjs[i].transform.position);
                        if ((distance < closest || closest == -1) && !playerFound && !brokenWallFound && !insideWalls)
                        {
                            closest = distance;
                            moveToObj = detectionZone.detectedObjs[i];
                        }
                    }
                    i++;
                }

                if (moveToObj != null)
                {
                    MoveTo(moveToObj);
                }
            }
            // If nothing is found, move to player
            else
            {
                MoveTo(player.GetComponent<Collider2D>());
            }
        }
        else // Else stop moving and die
        {
            //MoveTo(gameObject.GetComponent<Collider2D>());
            moveSpeed = 0;
            agent.ResetPath();
        }
    }

    private void MoveTo(Collider2D obj)
    {
        // AI Pathfinding movement
        target = obj.transform;
        agent.SetDestination(target.position);

        // Calculate direction to target for rotation
        Vector2 direction = (obj.transform.position - transform.position).normalized;

        // Calculate the distance from the target
        float distance = Vector3.Distance(transform.position, obj.transform.position);

        agent.isStopped = false;
        animator.SetBool("isWalking", true);

        // If the enemy has not gotten close enough to a repaired wall or the player, continue moving towards it
        if ((distance > maxDistanceToObject && !brokenWallFound) || distance > maxDistanceToObject && insideWalls)
        {
            // Move towards detected object
            agent.isStopped = false;
            agent.SetDestination(target.position);
            //rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
        }
        // Else if the enemy finds a broken wall, make them go to the center and not stop at a distance
        else if (brokenWallFound && !insideWalls)
        {
            agent.isStopped = false;
            agent.SetDestination(target.position);
            if (agent.remainingDistance <= 0.5)
            {
                insideWalls = true;
                agent.SetDestination(player.GetComponent<Collider2D>().transform.position);
            }
        }
        // Otherwise stop movement
        else
        {
            agent.isStopped = true;
            animator.SetBool("isWalking", false);
        }

        // Adjust bulletSpawnPoint
        Vector2 directionBullet = (obj.transform.position - bulletSpawnPoint.transform.position).normalized;
        bulletSpawnPoint.GetComponent<Rigidbody2D>().MovePosition(rb.position + directionBullet * moveSpeed * Time.fixedDeltaTime);

        // Using mouse position on screen, set direction to fire bullet
        Vector2 lookDir = player.GetComponent<Rigidbody2D>().position - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        //rb.rotation = angle;
        //bulletSpawnPoint.GetComponent<Rigidbody2D>().rotation = angle;

        // Rotate towards detected object
        float rot_z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bulletSpawnPoint.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);

        if (direction != Vector2.zero)
        {
            animator.SetFloat("X", direction.x);
            animator.SetFloat("Y", direction.y);
        }
        if (direction.x > 0 && !facingRight)
        {
            FlipDirection();
        }
        if (direction.x < 0 && facingRight)
        {
            FlipDirection();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (healthBase.isAlive)
        {
            // Cast a 2D line that retrieves all the hits
            RaycastHit2D[] hits2d = Physics2D.RaycastAll(bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.up, range);
            //Debug.DrawRay(bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.up * range, Color.green, 20);

            // For each hit found, check if it was the player, if so, then shoot a bullet
            foreach (RaycastHit2D hit2d in hits2d)
            {
                if (hit2d.collider != null && hit2d.collider.isTrigger && hit2d.collider.GetType().ToString() == "UnityEngine.BoxCollider2D" && hit2d.collider.name == "Player" && insideWalls)
                {
                    GameObject hitObject = hit2d.transform.gameObject;
                    if (hitObject.tag == "Player")
                    {
                        // Shoots a bullet wherever enemy is facing if bullet doesn't exist
                        if (canShoot)
                        {
                            StartCoroutine(FireRate());
                            //Debug.DrawRay(firedBullet.transform.position, firedBullet.transform.up * 20f, Color.red, 20);
                        }
                    }
                }
                else if (!hit2d.collider.isTrigger && hit2d.collider.GetType().ToString() == "UnityEngine.BoxCollider2D" && hit2d.transform.gameObject.tag == "Wall" && !insideWalls)
                {
                    // Shoots a bullet wherever enemy is facing if bullet doesn't exist
                    if (canShoot)
                    {
                        StartCoroutine(FireRate());
                        //Debug.DrawRay(firedBullet.transform.position, firedBullet.transform.up * 20f, Color.red, 20);
                    }
                }
            }
        }
    }

    // Allows for adjusting the fire rate
    IEnumerator FireRate()
    {
        canShoot = false;
        animator.SetTrigger("shoot");
        Invoke("ShootBullet", 0.4f);
        
        // Randomize the fire rate for more variation
        float actualRate = Random.Range(fireRate - fireRateRandomness, fireRate + fireRateRandomness);
        if (actualRate < 1.2f)
        {
            actualRate = 1.2f;
        }
        yield return new WaitForSeconds(actualRate);
        canShoot = true;
    }

    void ShootBullet()
    {
        GameObject firedBullet = Instantiate(bulletPrefab) as GameObject;
        PlayGunshot();
        firedBullet.GetComponent<BulletController>().EnableDestroy(firedBullet, delayToRemoveBullet);
        firedBullet.layer = 6;
        firedBullet.GetComponent<BulletController>().damage = bulletDamage;
        firedBullet.transform.position = bulletSpawnPoint.transform.TransformPoint(Vector3.up * bulletSpawnAdjust);
        firedBullet.transform.rotation = bulletSpawnPoint.transform.rotation;
        firedBullets.Append(firedBullet);
        animator.ResetTrigger("shoot");
    }

    void FlipDirection()
    {
        Vector3 currentScale = rb.transform.localScale;
        currentScale.x *= -1;
        rb.transform.localScale = currentScale;

        facingRight = !facingRight;
    }

    private void PlayGunshot()
    {
        gunshotSound.clip = gunshotClips[Random.Range(0, gunshotClips.Length)];
        gunshotSound.volume = Random.Range(1 - volumeChangeMultiplier, 1);
        gunshotSound.pitch = Random.Range(1 - pitchChangeMultiplier, 1 + pitchChangeMultiplier);
        gunshotSound.PlayOneShot(gunshotSound.clip);
    }
}
