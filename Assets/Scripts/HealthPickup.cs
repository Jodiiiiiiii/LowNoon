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
        if (collider.tag == "Player"){
            GameManager.Instance.PlayerData.CurrHealth++;
            Destroy(gameObject);
        }
        
    }
}
