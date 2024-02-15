using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    // Enemy health stored
    public float HealthLevel = 5;
    // Damaging object stored (with some amount of damage)
    [Tooltip("stores prefab of damaging object")] public GameObject DamagerObject;

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
        DamagerObject = collision.gameObject;

        if (DamagerObject.tag == "bullet")
        {
            //Die
            BulletStats damage = DamagerObject.GetComponent<BulletStats>();

            HealthLevel -= damage.DamageLevel;

            if (HealthLevel <= 0)
            {
                Destroy(gameObject);
                Destroy(DamagerObject); 
                
            }

            
            

            

            

        }
    }

    
}
