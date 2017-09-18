using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundDetector : MonoBehaviour {

    EdgeCollider2D ec2d;
    AudioSource audioSource;

    // Use this for initialization
    void Start()
    {
        ec2d = GetComponent<EdgeCollider2D>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (PlayerController.playerState != PlayerController.PlayerState.PLAYING)
        {
            audioSource.Stop();
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            audioSource.Play();
        }
    }
}
