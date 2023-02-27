using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public int distanceToShoot = 20;

    private GameObject firedBullet;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Cast a 2D line that retrieves all the hits
        RaycastHit2D[] hits2d = Physics2D.RaycastAll(transform.position, transform.up, distanceToShoot);
        Debug.DrawLine(transform.position, transform.position + new Vector3(distanceToShoot, 0, 0));
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
                        firedBullet.transform.position = transform.TransformPoint(Vector3.forward * 1.5f);
                        firedBullet.transform.rotation = transform.rotation;
                    }
                }
            }
        }
    }
}
