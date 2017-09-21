using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartEnemiesDetector : MonoBehaviour {

    Rigidbody2D rb2d;
    BoxCollider2D bc2d;

	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        bc2d = GetComponent<BoxCollider2D>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "StartEnemy")
        {
            Destroy(col.gameObject);
        }
    }
}
