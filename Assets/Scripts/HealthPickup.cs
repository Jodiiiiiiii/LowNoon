using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider){
        if (collider.CompareTag("Player")){
            if(GameManager.Instance.PlayerData.CurrHealth < GameManager.Instance.PlayerData.MaxHealth){
            GameManager.Instance.PlayerData.CurrHealth++;
            }
            Destroy(this.gameObject);
        }
    
    }

}
