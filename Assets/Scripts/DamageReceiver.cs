using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageReceiver : MonoBehaviour
{
    // Enemy health stored
    [Tooltip("Total amount of starting health")] public float HealthLevel = 5;
    [Tooltip("Whether this class should destroy the GameObject when HP = 0 (or let an animator do it)")] public bool IsDirectlyDestroyed;
    public bool IsImmune;  // Boolean used to toggle an invincibility state (i-frames)
    [Tooltip("Likelihood that object spawns health pickup")] public float DropRate = 1.0f;
    public GameObject SpawnObject;
    public bool IsGoldenBarrel;
    public GameObject Player;
    double chance;
    GameObject pickup;


    // Start is called before the first frame update
    void Start()
    {
        IsImmune = false;
        chance = Random.Range(0,1.0f);
        if(IsGoldenBarrel){
            pickup = Instantiate(SpawnObject);
            pickup.transform.position = gameObject.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject DamagerObject = collision.gameObject;

        if (DamagerObject.CompareTag("PlayerBullet"))
        {
            BulletStats bulletStats = DamagerObject.GetComponent<BulletStats>();

            if (!IsImmune)
            {
                // destroy damage receiver only if it reaches 0 health
                HealthLevel -= bulletStats.DamageLevel;
                if (HealthLevel <= 0)
                {
                    if(IsDirectlyDestroyed)
                        Destroy(gameObject);
                    if(IsGoldenBarrel){
                        
                        pickup.transform.rotation = Player.transform.rotation;
                        
                        
                        GameObject pickup2 = Instantiate(SpawnObject);
                        pickup2.transform.position = pickup.transform.position + (3.0f * Player.transform.right);
                        
                        
                        
                        StartCoroutine(DoUpgradeMove(pickup2));//, (-1.0f * Player.transform.right)));
                        
                        
                        GameObject pickup3 = Instantiate(SpawnObject);
                        pickup3.transform.position = pickup.transform.position + (-3.0f * Player.transform.right);
                        
                        
                        
                        
                        StartCoroutine(DoUpgradeMove(pickup3));//, Player.transform.right));
                        
    
                    }
                    else if(chance < DropRate)
                        
                        Spawn(); 
                    }
                        
                }

                
            }

        }
    private IEnumerator DoUpgradeMove(GameObject item){//}, Vector3 direction){
        float goalPosition = 3.0f;
        float increment = 0.1f;
        //Vector3 change = (increment*direction);
        float counter = 0.0f;

        while(counter <= goalPosition){
            //item.transform.position = (increment*direction) + item.transform.position;
            //counter += increment;
            //Debug.Log(counter);
            counter++;
            yield return null;
        }

        yield return null;
        
    }

    private void Spawn(){
        
        GameObject healthPickup = Instantiate(SpawnObject);
        healthPickup.transform.position = gameObject.transform.position;
        
        
        
    }
}

