using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    bool explosionInit = false;
    bool explosionTriggered = false;
    float explosionStart = 0f;

    int fuel = 100;
    float fuelDecreaseInit = Time.realtimeSinceStartup;
    bool noFuel = false;

    public static int score { get; set; } 

    public Text FuelValue;
    Text fuelValue;
    public Text SpeedValue;
    Text speedValue;
    public Text ScoreValue;
    Text scoreValue;
    public Text MessageValue;
    Text messageValue;
    public Image MessageBackground;
    Image messageBackground;
    
    // Use this for initialization
    void Start () {
        playerState = PlayerState.READY; // TODO: zmiana na ready
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        fuelValue = FuelValue.GetComponent<Text>();
        speedValue = SpeedValue.GetComponent<Text>();
        scoreValue = ScoreValue.GetComponent<Text>();
        messageValue = MessageValue.GetComponent<Text>();
        messageValue.text = "";
        messageBackground = MessageBackground.GetComponent<Image>();
        messageBackground.enabled = false;

        score = 0;
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
                audioSources[6].Stop();
                speed = 0;
                xAxis = 0;
                messageValue.text = "CHECK POINT";
                messageBackground.enabled = true;
                break;
            case PlayerState.PLAYING:
                if (!audioSources[0].isPlaying)
                {
                    audioSources[0].Play();
                }
                if (!noFuel)
                {
                    speed += speedUpdate(speed);
                }
                xAxis = xAxisUpdate(skid);
                audioSources[0].pitch = 1 + (speed / 400);
                SkidUpdate();
                ExplosionChecker();
                FuelReduction();
                rb2d.velocity = new Vector2(xAxis, speed);
                break;
            case PlayerState.GAMEOVER:
                audioSources[0].Stop();
                audioSources[1].Stop();
                audioSources[2].Stop();
                audioSources[3].Stop();
                audioSources[4].Stop();
                audioSources[5].Stop();
                audioSources[6].Stop();
                audioSources[7].Stop();
                audioSources[8].Stop();
                animator.SetBool("Skid", false);
                animator.SetBool("CircleSkidLeft", false);
                messageValue.text = "GAME OVER";
                messageBackground.enabled = true;
                break;
        }

        if (fuel >= 0) fuelValue.text = fuel.ToString();
        else fuelValue.text = "0";
        if (speed >= 0) speedValue.text = speed.ToString();
        else speedValue.text = "0";
        if (score >= 0) scoreValue.text = score.ToString();
        else scoreValue.text = "0";
    }

    // Fixed Update
    void FixedUpdate()
    {
        Debug.Log("FUEL: " + fuel); // TODO: tymczasowe - do usunięcia w wersji RELEASE
    }

    // On Trigger Enter 2D
    void OnTriggerEnter2D(Collider2D col)
    {
        //W momencie kiedy samochód gracza najedzie na dziurę, wywoła trigger, który spowoduje wpadnięcie w poślizg
        if (col.gameObject.CompareTag("Hole"))
        {
            skid = true;
            skidInit = Time.realtimeSinceStartup;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Bonus")
        {
            Destroy(col.gameObject);
            audioSources[1].Play();
            fuel += 6;
            score += 1000;
        }
        if (col.gameObject.tag == "Finish")
        {
            playerState = PlayerState.FINISH;

            audioSources[0].Stop();
            audioSources[1].Stop();
            audioSources[3].Stop();
            audioSources[4].Stop();
            audioSources[5].Stop();
            audioSources[6].Stop();
            audioSources[7].Stop();
            audioSources[8].Stop();

            audioSources[2].Play();
        }
        if (col.gameObject.tag == "Bound" && speed > 200f)
        {
            explosionInit = true;
            explosionTriggered = true;
            explosionStart = Time.realtimeSinceStartup;
        }
        if (col.gameObject.CompareTag("EnemyCar"))
        {
            skid = true;
            skidInit = Time.realtimeSinceStartup;
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
    
    private void SkidUpdate()
    {
        if (skid)
        {
            if (!skidStart)
            {
                audioSources[6].Play();
                skidLeft = Random.Range(0, 2);
                skidStart = true;
                animator.SetBool("Skid", skid);
            }
            if (skidLeft == 0) xAxis = 25f;
            else xAxis = -25f;
            if ((Time.realtimeSinceStartup - skidInit) < 1 && speed < 320f && !circleSkid)
            {
                skid = false;
                skidStart = false;
                animator.SetBool("Skid", skid);
                audioSources[6].Stop();
            }
            if ((Time.realtimeSinceStartup - skidInit) > 1 && speed > 320f && !circleSkid)
            {
                circleSkid = true;
                circleSkidStart = true;
                animator.SetBool("CircleSkidLeft", circleSkid);
            }
            if (circleSkid)
            {
                if (speed < 4f)
                {
                    circleSkid = false;
                    circleSkidStart = false;
                    skid = false;
                    skidStart = false;
                    audioSources[6].Stop();
                    animator.SetBool("CircleSkidLeft", circleSkid);
                    animator.SetBool("Skid", skid);
                }
            }
        }
    }

    private void ExplosionChecker()
    {
        if (explosionInit)
        {
            speed = 0;
            xAxis = 0;
            if (explosionTriggered)
            {
                animator.SetTrigger("Explosion");
                audioSources[7].Play();
                explosionTriggered = false;
            }
            if ((Time.realtimeSinceStartup - explosionStart) > 2f)
            {
                explosionInit = false;
                fuel -= 5;
            }
            
        }
    }

    private void FuelReduction()
    {
        if ((Time.realtimeSinceStartup - fuelDecreaseInit) > .75f)
        {
            fuel--;
            fuelDecreaseInit = Time.realtimeSinceStartup;
        }
        if (fuel < 10)
        {
            if (!audioSources[8].isPlaying)
            {
                audioSources[8].Play();
            }
        }
        else if (audioSources[8].isPlaying)
        {
            audioSources[8].Stop();
        }
        if (fuel <= 0 && speed > 0)
        {
            noFuel = true;
            speed--;
        }
        if (noFuel && speed <= 0)
        {
            playerState = PlayerState.GAMEOVER;
        }
    }

}
