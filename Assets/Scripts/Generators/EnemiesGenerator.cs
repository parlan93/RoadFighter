using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesGenerator : MonoBehaviour {

    public GameObject[] enemies;

	// Use this for initialization
	void Start () {
        GenerateEnemies();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void GenerateEnemies()
    {
        for (int i = 0; i < 60; i++)
        {
            int randomEnemy = Random.Range(0, enemies.Length);
            int randomX = Random.Range(-30, 30);
            int randomY = Random.Range(300, 500);
            var pos = new Vector3(randomX, (i+2) * randomY, transform.position.z);
            Instantiate(enemies[randomEnemy], pos, Quaternion.Euler(Vector3.zero));
        }
    }
}
