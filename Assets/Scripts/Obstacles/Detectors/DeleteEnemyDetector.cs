using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteEnemyDetector : MonoBehaviour {

    EdgeCollider2D ec2d;

	// Use this for initialization
	void Start () {
        ec2d = GetComponent<EdgeCollider2D>();
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            Destroy(transform.parent.gameObject);
        }
    }
}
