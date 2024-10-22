using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPlayerSpawnPosition : MonoBehaviour
{
    public GameObject PlayerObject;
    
    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance.IsMainMenuLoaded) // Prevents this from activating in the hub the first time
        {
            PlayerObject = GameObject.FindGameObjectWithTag("Player");
            PlayerObject.transform.position = gameObject.transform.position;
            PlayerObject.transform.rotation = gameObject.transform.rotation;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
