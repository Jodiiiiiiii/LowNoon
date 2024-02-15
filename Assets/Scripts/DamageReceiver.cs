using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageReceiver : MonoBehaviour
{
    // Enemy health stored
    [Tooltip("Total amount of starting health")] public float HealthLevel = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject DamagerObject = collision.gameObject;

        if (DamagerObject.CompareTag("bullet"))
        {
            BulletStats bulletStats = DamagerObject.GetComponent<BulletStats>();

            // destroy bullet always on impact with damage receiver
            Destroy(DamagerObject);

            // destroy damage receiver only if it reaches 0 health
            HealthLevel -= bulletStats.DamageLevel;
            if (HealthLevel <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    
}
