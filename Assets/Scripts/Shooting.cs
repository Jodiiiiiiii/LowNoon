using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject bullet;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) // shooting command AND not during cooldown time AND in stationary state
        {
            GameObject newBullet = Instantiate(bullet);

            newBullet.transform.position = transform.position + (0.5f * transform.forward); // somehow coming out of the gun 
            newBullet.GetComponent<Rigidbody>().AddForce(50f* transform.forward, ForceMode.Impulse); //experiment w this or with adding just a velocity
            
            



            // also shooting anim should play?
        }
    }
}
