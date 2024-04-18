using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script controls the spawning of tumbleweeds along a line on the x-axis.

public class TumbleweedSpawner : MonoBehaviour
{
    
    [SerializeField] private float _length = 25f;    // The length of the area that tumbleweeds can spawn from
    [SerializeField] private float _spawnIntervalMin;   // How often a new tumbleweed is spawned
    [SerializeField] private float _spawnIntervalMax;   // How often a new tumbleweed is spawned
    private float _timer;
    [SerializeField] private GameObject _tumbleweedPrefab;   // The tumbleweed to spawn

    void Start()
    {
        // Setting all of our variables using the inputs from the Inspector window
        _timer = Random.Range(_spawnIntervalMin, _spawnIntervalMax);
    }

    // Update is called once per frame
    void Update()
    {
        if(_timer < 0)
        {
            SpawnTumbleweed();
            _timer = Random.Range(_spawnIntervalMin, _spawnIntervalMax);
        }

        _timer -= Time.deltaTime;
    }

    void SpawnTumbleweed()
    {
        Vector3 spawnPos = transform.position + transform.right * Random.Range(-_length, _length);
        GameObject tumbleweed = Instantiate(_tumbleweedPrefab, spawnPos, _tumbleweedPrefab.transform.rotation);
        tumbleweed.transform.LookAt(tumbleweed.transform.position + transform.forward);
    }
}
