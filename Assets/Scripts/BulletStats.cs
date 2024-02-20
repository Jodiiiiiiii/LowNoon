using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// stores relevant bullet stats and handles expiration after lifetime duration
/// </summary>
public class BulletStats : MonoBehaviour
{

    [Tooltip("Amount of damage dealt on impact to damage reciever")] public int DamageLevel = 1;
    [Tooltip("Impulse force magnitude used to launch projectile")] public float InitialForce = 50.0f;
    [Tooltip ("How long before bullet is destroyed (seconds)")] public float BulletLifetime = 3.0f;
    
    float _timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        if(_timer > BulletLifetime)
        {
            Destroy(gameObject);
        }
    }

}
