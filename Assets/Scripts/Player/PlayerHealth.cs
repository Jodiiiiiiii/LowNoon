using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
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
        

        if (DamagerObject.CompareTag("EnemyBullet"))
        {
            BulletStats bulletStats = DamagerObject.GetComponent<BulletStats>();
            

            // destroy bullet always on impact with damage receiver
            Destroy(DamagerObject);
            


            GameManager.Instance.PlayerData.Armor -= bulletStats.DamageLevel;
            if (GameManager.Instance.PlayerData.Armor < 0)
            {
                GameManager.Instance.PlayerData.CurrHealth += GameManager.Instance.PlayerData.Armor;
                GameManager.Instance.PlayerData.Armor = 0;
            }
            
            // check if player reaches 0 health
            if (GameManager.Instance.PlayerData.CurrHealth <= 0)
            {
                // player dies?
            }
        }



        //if armor upgrade?? then currHealth += armor. then same effect happens?
        // += on the stats script lol?

    }
}
