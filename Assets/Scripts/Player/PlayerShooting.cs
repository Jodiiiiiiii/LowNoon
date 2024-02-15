using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    // Stores prefab of bullet object
    [Tooltip("stores prefab of bullet object")] public GameObject BulletObject;

    [Tooltip("how long before player can shoot again (seconds)")] public float CooldownTime = 0.0f;

    //Reference to player object script
    PlayerController _playerController;

    

    float timer = 0.0f;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    { 
        if (Input.GetKeyDown(KeyCode.Mouse0) && timer > CooldownTime) //&& playerController.State == CharacterState.Stationary) // shooting command AND not during cooldown time !!! AND in stationary state
        {
            GameObject newBullet = Instantiate(BulletObject);

            newBullet.transform.position = transform.position + (0.5f * transform.forward); // somehow coming out of the gun 
            newBullet.GetComponent<Rigidbody>().AddForce(newBullet.GetComponent<BulletStats>().InitialForce * transform.forward, ForceMode.Impulse);

            timer = 0.0f;

        }

        timer += Time.deltaTime;
    }
}
