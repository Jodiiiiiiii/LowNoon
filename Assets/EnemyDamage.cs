using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    // Enemy health stored
    public float healthLevel = 5;
    // Damaging object stored (with some amount of damage)
    public GameObject damager;
    // Start is called before the first frame update
    public float knockbackDistance = 10.0f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        damager = collision.gameObject;

        if (damager.tag == "bullet")
        {
            //Die?

            if (healthLevel == 1)
            {
                Destroy(gameObject);
                Destroy(damager); // if like a bullet or whatnot?
                
            }

            
            Bullet damage = damager.GetComponent<Bullet>();

            healthLevel -= damage.damage;

            //Knockback enemies
            gameObject.transform.Translate(-1.0f * knockbackDistance * gameObject.transform.forward);

            

        }
    }

    
}
