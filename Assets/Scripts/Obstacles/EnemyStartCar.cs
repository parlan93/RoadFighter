using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStartCar : EnemyVehicle
{
    
    protected override void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        bc2d = GetComponent<BoxCollider2D>();
        ec2d = GetComponent<EdgeCollider2D>();
    }

    protected override void Update()
    {
        UpdateComponent();
        if (PlayerController.playerState == PlayerController.PlayerState.PLAYING)
        {
            init = true;
        }
    }

    protected override void OnCollisionEnter2D(Collision2D col)
    {
        
    }

    protected override void OnTriggerEnter2D(Collider2D col)
    {
        
    }

}
