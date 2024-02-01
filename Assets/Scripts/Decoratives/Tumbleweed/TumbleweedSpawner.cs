using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script controls the spawning of tumbleweeds along a line on the x-axis.

public class TumbleweedSpawner : MonoBehaviour
{
    
    [SerializeField] private float _length = 25f;    // The length of the area that tumbleweeds can spawn from
    [SerializeField] private float _spawnInterval;   // How often a new tumbleweed is spawned
    [SerializeField] private GameObject _tumbleweedPrefab;   // The tumbleweed to spawn

    private float min;  // The leftmost extent of the spawn line
    private float max;  // The rightmost extent of the spawn line
    void Start()
    {
        // Setting all of our variables using the inputs from the Inspector window
        min = this.transform.position.x - _length;
        max = this.transform.position.x + _length;
        InvokeRepeating("SpawnTumbleweed", 0, _spawnInterval);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SpawnTumbleweed()
    {
        Instantiate(_tumbleweedPrefab, new Vector3(Random.Range(min, max + 1), this.transform.position.y, this.transform.position.z), new Quaternion());
    }
}
