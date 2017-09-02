using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    Rigidbody2D rb2d;
    Animator animator;

    float speed;
    float maxSpeed;
    float xAxis;

    bool skid;
    bool skidStart;
    bool circleSkid;
    bool circleSkidStart;
    float skidInit;
    int skidLeft;
    float circleSkidInit;
    int circleSkidLeft;
    
    // Use this for initialization
    void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        speed = 0.0f;
        maxSpeed = 400.0f;
        xAxis = 0f;

        skid = false;
        skidStart = false;
        circleSkid = false;
        circleSkidStart = false;
        skidInit = 0;
        skidLeft = 0;
        circleSkidInit = 0;
        circleSkidLeft = 0;
    }
	
	// Update is called once per frame
	void Update () {

        // Controller
        if (Input.GetButton("Accelerator") && !Input.GetButton("Brake") && speed < maxSpeed)
            speed += 2f;
        else if (speed > 0) speed -= 1f;
        if (Input.GetButton("Brake") && speed >= 0)
            speed -= 4f;
        if (Input.GetButton("Left") && !skid)
        {
            xAxis = -100f;
            if (Input.GetButton("Right")) xAxis = 0f;
        }
        if (Input.GetButton("Right") && !skid)
        {
            xAxis = 100f;
            if (Input.GetButton("Left")) xAxis = 0f;
        }   
        if (!Input.GetButton("Left") && !Input.GetButton("Right")) xAxis = 0f;
        if (skid)
        {
            if (!skidStart)
            {
                skidLeft = Random.Range(0, 2);
                skidStart = true;
                animator.SetBool("Skid", skid);
            }
            if (skidLeft == 0) xAxis = 25f;
            else xAxis = -25f;
            if ((Time.realtimeSinceStartup - skidInit) > 1 && speed > 300f && !circleSkid)
            {
                circleSkid = true;
                circleSkidStart = true;
                animator.SetBool("CircleSkidLeft", circleSkid);
            }
        }
        rb2d.velocity = new Vector2(xAxis, speed);

    }

    // Fixed Update
    void FixedUpdate()
    {
        Debug.Log("SPEED: " + speed);
    }

    // On Trigger Enter 2D
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Hole"))
        {
            skid = true;
            skidInit = Time.realtimeSinceStartup;
        }
    }
}
