using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinecartMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject StartPosition;
    public float Speed = 20f;
    private Rigidbody _rb;
    void Start()
    {
        _rb = this.GetComponent<Rigidbody>();
        transform.position = StartPosition.transform.position;
        transform.rotation = StartPosition.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        _rb.velocity = new Vector3(this._rb.velocity.x, this._rb.velocity.y, _speed);
        
    }

    void OnCollisionEnter(Collision collision){
        //if collision object is player, destroy the minecart thing and harm player? 
    }
}
