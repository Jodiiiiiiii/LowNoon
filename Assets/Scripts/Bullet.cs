using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float DamageLevel = 1.0f;
    public float InitialForce = 50.0f;
    [Tooltip ("how long before bullet is destroyed (seconds)")] public float BulletLifetime = 3.0f;
    float timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > BulletLifetime)
        {
            Destroy(gameObject);
        }
    }

    
}
