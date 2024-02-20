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
            GameManager.Instance.PlayerData.CurrHealth--;

            // destroy bullet always on impact with damage receiver
            Destroy(DamagerObject);
            Debug.Log(GameManager.Instance.PlayerData.CurrHealth);

            // check if player reaches 0 health
            GameManager.Instance.PlayerData.CurrHealth -= bulletStats.DamageLevel;
            if (GameManager.Instance.PlayerData.CurrHealth <= 0)
            {
                Debug.Log("dead");
            }
        }
    }
}
