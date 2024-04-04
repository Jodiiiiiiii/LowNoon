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
                Debug.Log("ow");

                //decrement health by dmg
                if (GameManager.Instance.PlayerData.Armor > 0) {
                    GameManager.Instance.PlayerData.Armor -= 1;
                }
                else {
                    GameManager.Instance.PlayerData.CurrHealth -= DamageAmount;
                    }

                
            }
            else if(obj.tag == "Enemy"){
                Debug.Log("enemy");
                // question mark question mark question mark
                healthLevel = obj.GetComponent<DamageReceiver>();
                healthLevel.HealthLevel -= (float)DamageAmount;

            }
        }
    }
}
