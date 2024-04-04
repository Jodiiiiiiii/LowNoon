using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinecartMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject StartPosition;
    public GameObject EndPosition;
    public float Speed = 1.0f;
    public GameObject ExplosionParticles;
    private Rigidbody _rb;
    void Start()
    {
        
        transform.position = new Vector3(StartPosition.transform.position.x, 1.0f, StartPosition.transform.position.z);
        transform.rotation = StartPosition.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position != EndPosition.transform.position){
            transform.position = Vector3.MoveTowards(transform.position, EndPosition.transform.position, Speed*Time.deltaTime);
        }
        
    }

    void OnCollisionEnter(Collision collision){
        if(collision.collider.tag == "Player" || collision.collider.tag == "Enemy"){
            Instantiate(ExplosionParticles, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(gameObject);
        }
    }

}
