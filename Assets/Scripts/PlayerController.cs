using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public enum PlayerState
    {
        READY,
        PLAYING,
        FINISH,
        GAMEOVER
    }

    public AudioSource[] audioSources;
    public static PlayerState playerState { get; set; }
    Rigidbody2D rb2d;
    Animator animator;
    float speed = 0f;
    float maxSpeed = 400f;
    float xAxis = 0f;
    
    bool skid = false;
    bool skidStart = false;
    bool circleSkid = false;
    bool circleSkidStart = false;
    float skidInit = 0;
    int skidLeft = 0;
    float circleSkidInit = 0f;
    int circleSkidLeft = 0;

    bool isStartCeremony = true;
    float startCeremonyInitTime = Time.realtimeSinceStartup;
    int sirensReady = 0;
    bool sirenGo = false;
    
    // Use this for initialization
    void Start () {
        playerState = PlayerState.READY; // TODO: zmiana na ready
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
    }
	
	// Update is called once per frame
	void Update () {

        switch(playerState)
        {
            case PlayerState.READY:
                if (Time.realtimeSinceStartup - startCeremonyInitTime > 5)
                {
                    if (!audioSources[4].isPlaying && sirensReady < 3)
                    {
                        audioSources[4].Play();
                        sirensReady++;
                    }
                    if (sirensReady >= 3 && !sirenGo && !audioSources[4].isPlaying)
                    {
                        audioSources[5].Play();
                        sirenGo = true;
                    }
                    if (sirenGo)
                    {
                        playerState = PlayerState.PLAYING;
                    }
                }
                break;
            case PlayerState.FINISH:
                audioSources[0].Stop();
                speed = 0.0000f;
                
                break;
            case PlayerState.PLAYING:
                if (!audioSources[0].isPlaying)
                {
                    audioSources[0].Play();
                }
                speed += speedUpdate(speed);
                xAxis = xAxisUpdate(skid);
                audioSources[0].pitch = 1 + (speed / 400);
                //TODO: skid
                rb2d.velocity = new Vector2(xAxis, speed);
                break;
        }
        
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

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Bonus")
        {
            Destroy(col.gameObject);
            audioSources[1].Play();
        }
        if (col.gameObject.tag == "Finish")
        {
            playerState = PlayerState.FINISH;
            audioSources[2].Play();
        }
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
