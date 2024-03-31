using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenItemDestroyer : MonoBehaviour
{
    public GameObject WoodParticles;
    private DamageReceiver _receiver;
    // Start is called before the first frame update
    void Start()
    {
        _receiver = gameObject.GetComponent<DamageReceiver>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_receiver.HealthLevel <= 0)
        {
            Instantiate(WoodParticles, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(gameObject);
        }
    }
}
