using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinecartSpawner : MonoBehaviour
{
    [SerializeField] private float _spawnIntervalMin;   
    [SerializeField] private float _spawnIntervalMax;   
    private float _timer;
    [SerializeField] private GameObject _minecartPrefab;
    [SerializeField, Tooltip("Height of spawner object for best alignment with tracks")] private float _spawnerHeight = 0.8f;
   
    void Start()
    {
        // set spawner height
        transform.position = new Vector3(transform.position.x, _spawnerHeight, transform.position.z);

        _timer = Random.Range(_spawnIntervalMin, _spawnIntervalMax);
    }

    // Update is called once per frame
    void Update()
    {
        if(_timer < 0)
        {
            SpawnMinecart();
            _timer = Random.Range(_spawnIntervalMin, _spawnIntervalMax);
        }

        _timer -= Time.deltaTime;
    }

    void SpawnMinecart()
    {
        //Spawns minecart in position and orientation of spawner object
        Instantiate(_minecartPrefab, transform.position, transform.rotation);
    }
}
