using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageReceiver : MonoBehaviour
{
    // Enemy health stored
    [Tooltip("Total amount of starting health")] public float HealthLevel = 5;
    [Tooltip("Whether this class should destroy the GameObject when HP = 0 (or let an animator do it)")] public bool IsDirectlyDestroyed;
    public bool IsImmune;  // Boolean used to toggle an invincibility state (i-frames)

    // Start is called before the first frame update
    void Start()
    {
        IsImmune = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        GameObject DamagerObject = collision.gameObject;

        if (DamagerObject.CompareTag("PlayerBullet"))
        {
            BulletStats bulletStats = DamagerObject.GetComponent<BulletStats>();

            if (!IsImmune)
            {
                // destroy damage receiver only if it reaches 0 health
                HealthLevel -= bulletStats.DamageLevel;
                if (HealthLevel <= 0 && IsDirectlyDestroyed)
                {
                    Destroy(gameObject);
                }
            }
            
        }
    }

    
}
