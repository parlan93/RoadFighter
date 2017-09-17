using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    // Stałe
    private const int MAP_START_FIXED_FRAGMENTS = 6;
    private const int MAP_FRAGMENTS = 60;
    // TODO: dodanie na końcu trasy fragmentów z linią mety
    private const int MAP_END_FIXED_FRAGMENTS = 0; 

    // Tablica prefabrykatów, z których złożona ma być mapa
    public GameObject[] mapPrefabs;

    // Use this for initialization
    void Start () {
        GenerateMap();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /**
     * Generator mapy, który losuje prefabrykat i dołącza go do istniejących fragmentów
     */
    void GenerateMap()
    {
        for (int i = MAP_START_FIXED_FRAGMENTS; i < MAP_FRAGMENTS; i++)
        {
            // Losuje prefabrykat mapy
            int randomMapFragment = Random.Range(0, mapPrefabs.Length);
            // Ustala pozycje na mapie, w której ma pojawić się losowy prefabrykat
            var pos = new Vector3(0, i * 288f, transform.position.z);
            // Tworzy klon prefabrykatu
            Instantiate(mapPrefabs[randomMapFragment], pos, Quaternion.Euler(Vector3.zero)); 
        }
    }
}
