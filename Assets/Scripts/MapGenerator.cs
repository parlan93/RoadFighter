using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    public GameObject[] mapPrefabs;

    // Use this for initialization
    void Start () {
        GenerateMap();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void GenerateMap()
    {
        for (int i = 6; i < 60; i++)
        {
            int randomMapFragment = Random.Range(0, mapPrefabs.Length);
            var pos = new Vector3(0, i * 288f, transform.position.z);
            Instantiate(mapPrefabs[randomMapFragment], pos, Quaternion.Euler(Vector3.zero));
        }
    }
}
