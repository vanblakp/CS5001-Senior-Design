using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    public string playerTarget = "Player";
    public string wallTarget = "Wall";
    public List<Collider2D> detectedObjs = new List<Collider2D>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == playerTarget || collision.gameObject.tag == wallTarget)
        {
            detectedObjs.Add(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == playerTarget || collision.gameObject.tag == wallTarget)
        {
            detectedObjs.Remove(collision);
        }
    }
}
