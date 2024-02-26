using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public bool InvulnerableState = false;
    public float InvulnerableTime = 5.0f;
    float timer = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > InvulnerableTime)
        {
            InvulnerableState = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject DamagerObject = collision.gameObject;
        

        if (DamagerObject.CompareTag("EnemyBullet"))
        {
            BulletStats bulletStats = DamagerObject.GetComponent<BulletStats>();
            

            // destroy bullet always on impact with player
            Destroy(DamagerObject);


            if (!InvulnerableState)
            {
                GameManager.Instance.PlayerData.Armor -= bulletStats.DamageLevel;
                if (GameManager.Instance.PlayerData.Armor < 0)
                {
                    GameManager.Instance.PlayerData.CurrHealth += GameManager.Instance.PlayerData.Armor;
                    GameManager.Instance.PlayerData.Armor = 0;
                }
                InvulnerableState = true;
                timer = 0.0f; // resets timer
            }
            
            // check if player reaches 0 health
            if (GameManager.Instance.PlayerData.CurrHealth <= 0)
            {
                // player dies
            }
        }

        

    }
}
