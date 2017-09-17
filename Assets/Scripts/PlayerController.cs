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

        // Gaz, hamulec
        speed += speedUpdate(speed);
        // Prawo, lewo
        xAxis = xAxisUpdate(skid);

        // Poślizg
        // TODO: skid

        // Zmiana w fizyce samochodu gracza
        rb2d.velocity = new Vector2(xAxis, speed);

    }

    // Fixed Update
    void FixedUpdate()
    {
        Debug.Log("SPEED: " + speed); // TODO: tymczasowe - do usunięcia w wersji RELEASE
    }

    // On Trigger Enter 2D
    void OnTriggerEnter2D(Collider2D col)
    {
        // TODO: skidTrigger
        // W momencie kiedy samochód gracza najedzie na dziurę, wywoła trigger, który spowoduje wpadnięcie w poślizg
        /*if (col.gameObject.CompareTag("Hole"))
        {
            skid = true;
            skidInit = Time.realtimeSinceStartup;
        }*/
    }

    /////////////////////////////////////////////////////////////////////////////////////////////
    ///
    /// Metody dotyczące fizyki samochodu
    /// 
    /////////////////////////////////////////////////////////////////////////////////////////////

    private float speedUpdate(float speed)
    {
        if (Input.GetButton("Accelerator") && !Input.GetButton("Brake") && speed < maxSpeed)
        {
            return 2f;
        }
        else
        {
            if (!Input.GetButton("Accelerator") && !Input.GetButton("Brake") && speed > 0)
            {
                return -1f;
            } 
            else
            {
                if (Input.GetButton("Brake") && speed >= 0)
                {
                    return -4f;
                }
                else
                {
                    return 0f;
                }
            }
        }
    }

    private float xAxisUpdate(bool skid)
    {
        if (Input.GetButton("Left") && !skid)
        {
            if (Input.GetButton("Right"))
            {
                return 0f;
            }
            return -100f;
        }
        if (Input.GetButton("Right") && !skid)
        {
            if (Input.GetButton("Left"))
            {
                return 0f;
            }
            return 100f;
        }
        return 0f;
    }

    // TODO: skidUpdate and skidChecker
    private void skidUpdate()
    {
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
    }

}
