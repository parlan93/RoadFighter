using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyVehicle : MonoBehaviour {

    protected float speed = 280f;
    protected Vector2 v2 = new Vector2(0, 1);
    protected Rigidbody2D rb2d;
    protected BoxCollider2D bc2d;
    protected EdgeCollider2D ec2d;
    protected AudioSource audioSource;
    protected AudioClip audioClip;
    protected bool init = false;

    protected abstract void Start();
    protected abstract void Update();
    protected abstract void OnCollisionEnter2D(Collision2D col);
    protected abstract void OnTriggerEnter2D(Collider2D col);

	// Use this for initialization
	protected void StartComponent() {
        rb2d = GetComponent<Rigidbody2D>();
        bc2d = GetComponent<BoxCollider2D>();
        ec2d = GetComponent<EdgeCollider2D>();
        audioSource = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	protected void UpdateComponent () {
        if (init)
        {
            rb2d.velocity = v2.normalized * speed;
        } else
        {
            rb2d.velocity = v2.normalized * 0;
        }
	}
    
    protected void OnCollisionEnter2DComponent(Collision2D col)
    {
        if (!audioSource.isPlaying && col.gameObject.tag == "Player")
        {
            audioSource.Play();
        }
    }

    protected void OnTriggerEnter2DComponent(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            init = true;
        }
    }

}
