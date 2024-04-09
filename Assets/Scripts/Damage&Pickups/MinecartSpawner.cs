using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinecartSpawner : MonoBehaviour
{
    [SerializeField] private float _spawnIntervalMin;   
    [SerializeField] private float _spawnIntervalMax;   
    private float _timer;
    [SerializeField] private GameObject _minecartPrefab;

   
    void Start()
    {
        
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
        GameObject minecart = Instantiate(_minecartPrefab, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), transform.rotation);
        
    }
}
