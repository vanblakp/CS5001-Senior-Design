using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public AudioSource soundPlayerClickIn;
    public AudioSource soundPlayerClickOut;
    public void PlayClickInSound()
    {
        soundPlayerClickIn.Play();
    }

    public void PlayClickOutSound()
    {
        soundPlayerClickOut.Play();
    }
}
