using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpawnAdjust = 1.25f;
    public int range = 20;
    public int maxDistanceToObject = 10;
    public float moveSpeed = 1;
    public float fireRate = 1f;
    public float fireRateRandomness = 0.5f;

    [SerializeField] private GameObject player;

    private Rigidbody2D rb;
    private DetectionZone detectionZone;
    private GameObject firedBullet;
    private bool canShoot = true;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        detectionZone = GetComponent<DetectionZone>();
    }

    private void FixedUpdate()
    {
        if (detectionZone.detectedObjs.Count > 0)
        {
            // Use simple AI Pathfinding to improve and implement this
            // https://www.youtube.com/watch?v=W-NIYi1t16Q
            // Adjust this to target a wall if no player in site or if there's no path to the player
            MoveTo(detectionZone.detectedObjs[0]);

            // Otherwise target the player
        }
        else
        {
            MoveTo(player.GetComponent<Collider2D>());
        }
    }

    private void MoveTo(Collider2D obj)
    {
        // Calculate direction to target
        Vector2 direction = (obj.transform.position - transform.position).normalized;

        // Calculate the distance from the target
        float distance = Vector3.Distance(transform.position, obj.transform.position);

        // If the enemy has not gotten close enough, continue moving towards it
        if (distance > maxDistanceToObject)
        {
            // Move towards detected object
            //rb.AddForce(direction * moveSpeed * Time.deltaTime);
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
        }

        // Rotate towards detected object
        float rot_z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
    }

    // Update is called once per frame
    void Update()
    {
        // Cast a 2D line that retrieves all the hits
        RaycastHit2D[] hits2d = Physics2D.RaycastAll(transform.position, transform.up, range);
        //Debug.DrawRay(transform.position, transform.up * distanceToShoot, Color.green, 20);

        // For each hit found, check if it was the player, if so, then shoot a bullet
        foreach (RaycastHit2D hit2d in hits2d)
        {
            if (hit2d.collider != null && hit2d.collider.isTrigger && hit2d.collider.GetType().ToString() == "UnityEngine.BoxCollider2D" && hit2d.collider.name == "Player")
            {
                GameObject hitObject = hit2d.transform.gameObject;
                if (hitObject.tag == "Player" || hitObject.tag == "Wall")
                {
                    // Shoots a bullet wherever enemy is facing if bullet doesn't exist
                    if (firedBullet == null && canShoot)
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

        firedBullet = Instantiate(bulletPrefab) as GameObject;
        firedBullet.layer = 6;
        firedBullet.transform.position = transform.TransformPoint(Vector3.up * bulletSpawnAdjust);
        firedBullet.transform.rotation = transform.rotation;

        // Randomize the fire rate for more variation
        float actualRate = Random.Range(fireRate - fireRateRandomness, fireRate + fireRateRandomness);

        yield return new WaitForSeconds(actualRate);
        canShoot = true;
    }
}
