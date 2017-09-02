using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour {

    public GameObject[] spawnPoints;

	// Use this for initialization
	void Start () {
        GenerateObstacle();
	}

    void GenerateObstacle()
    {
        int random = Random.Range(0, spawnPoints.Length);
        int i = 0;
        foreach (GameObject go in spawnPoints)
        {
            if (i == random) go.SetActive(true);
            else go.SetActive(false);
            i++;
        }
        
    }
}
