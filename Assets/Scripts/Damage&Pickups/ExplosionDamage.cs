using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDamage : MonoBehaviour
{
    public float ExplosionRadius;
    public int DamageAmount;

    void OnDestroy(){
        Collider[] damagedObjects = Physics.OverlapSphere(transform.position, ExplosionRadius);
        foreach (var obj in damagedObjects)
        {
            if(obj.tag == "Player"){
                obj.GetComponent<PlayerHealth>().ExplosionDmg = DamageAmount;
            }
            else if(obj.TryGetComponent<DamageReceiver>(out DamageReceiver receiver)){
                receiver.HealthLevel -= (float) DamageAmount;
            }
        }
    }
}
