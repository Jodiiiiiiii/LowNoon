using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script controls the spawning of tumbleweeds along a line on the x-axis.

public class TumbleweedSpawner : MonoBehaviour
{
    
    [SerializeField] private float length = 25f;    // The length of the area that tumbleweeds can spawn from
    [SerializeField] private float spawnInterval;   // How often a new tumbleweed is spawned
    [SerializeField] private GameObject tumbleweedPrefab;   // The tumbleweed to spawn

    private float spawnTimer;   // The timer we use to count down to the next spawning of a tumbleweed
    private float min;  // The leftmost extent of the spawn line
    private float max;  // The rightmost extent of the spawn line
    void Start()
    {
        // Setting all of our variables using the inputs from the Inspector window
        min = this.transform.position.x - length;
        max = this.transform.position.x + length;
        spawnTimer = spawnInterval;
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            spawnTimer = spawnInterval;
            Instantiate(tumbleweedPrefab, new Vector3(Random.Range(min, max + 1), this.transform.position.y, this.transform.position.z), new Quaternion());
        }
    }
}
