using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDamage : MonoBehaviour
{
    public float ExplosionRadius;
    public int DamageAmount;
    private const int PlayerExplosionDamage = 2; // hard coded to prevent player from being one-shot

    void OnDestroy(){
        Collider[] damagedObjects = Physics.OverlapSphere(transform.position, ExplosionRadius);
        foreach (var obj in damagedObjects)
        {
            if(obj.tag == "Player"){
                obj.GetComponent<PlayerHealth>().ExplosionDmg = PlayerExplosionDamage;
            }
            else if(obj.TryGetComponent<DamageReceiver>(out DamageReceiver receiver)){
                receiver.HealthLevel -= (float) DamageAmount;
            }
        }
    }
}
