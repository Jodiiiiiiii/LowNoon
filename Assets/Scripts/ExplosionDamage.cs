using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDamage : MonoBehaviour
{
    public float ExplosionRadius;
    public int DamageAmount;
    
    // Start is called before the first frame update
    void Start()
    {
     
     
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy(){
        Collider[] damagedObjects = Physics.OverlapSphere(transform.position, ExplosionRadius);
        foreach (var obj in damagedObjects)
        {
            if(obj.tag == "Player"){
                obj.GetComponent<PlayerHealth>().ExplosionDmg = DamageAmount;
            }
            else if(obj.GetComponent<DamageReceiver>() != null){
                //Debug.Log("enemy");
                // question mark question mark question mark
                DamageReceiver healthLevel = obj.GetComponent<DamageReceiver>();
                healthLevel.HealthLevel -= (float)DamageAmount;
                //Debug.Log("Other health: " + healthLevel.HealthLevel);

            }
        }
    }
    
}