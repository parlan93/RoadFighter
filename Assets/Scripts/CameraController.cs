using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    // Zmienne
    Camera camera;

	// Use this for initialization
	void Start () {
        camera = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        // Przypina kamerę do samochodu gracza
        camera.transform.position = new Vector3(0f, transform.position.y, transform.position.z);
	}
}
