using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpawnAdjust = 1.25f;
    public int distanceToShoot = 20;
    public float moveSpeed = 1;

    private Rigidbody2D rb;
    private DetectionZone detectionZone;
    private GameObject firedBullet;


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
            Collider2D detectedObject0 = detectionZone.detectedObjs[0];
            // Calculate direction to target
            Vector2 direction = (detectedObject0.transform.position - transform.position).normalized;

            // Move towards detected object
            //rb.AddForce(direction * moveSpeed * Time.deltaTime);
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);

            // Rotate towards detected object
            float rot_z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Cast a 2D line that retrieves all the hits
        RaycastHit2D[] hits2d = Physics2D.RaycastAll(transform.position, transform.up, distanceToShoot);
        //Debug.DrawRay(transform.position, transform.up * distanceToShoot, Color.green, 20);

        // For each hit found, check if it was the player, if so, then shoot a bullet
        foreach (RaycastHit2D hit2d in hits2d)
        {
            if (hit2d.collider != null && hit2d.collider.isTrigger && hit2d.collider.GetType().ToString() == "UnityEngine.BoxCollider2D" && hit2d.collider.name == "Player")
            {
                GameObject hitObject = hit2d.transform.gameObject;
                if (hitObject.tag == "Player")
                {
                    // Shoots a bullet wherever enemy is facing if bullet doesn't exist
                    if (firedBullet == null)
                    {
                        firedBullet = Instantiate(bulletPrefab) as GameObject;
                        firedBullet.layer = 6;
                        firedBullet.transform.position = transform.TransformPoint(Vector3.up * bulletSpawnAdjust);
                        firedBullet.transform.rotation = transform.rotation;
                        //Debug.DrawRay(firedBullet.transform.position, firedBullet.transform.up * 20f, Color.red, 20);
                    }
                }
            }
        }
    }
}
