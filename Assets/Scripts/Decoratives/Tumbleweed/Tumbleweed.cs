using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class controls the behavior of a tumbleweed, namely its speed and lifetime.

public class Tumbleweed : MonoBehaviour
{
    [SerializeField] private float _speed = 14f; // How fast the tumbleweed is going
    [SerializeField] private float _lifetime = 45f;  // How long the tumbleweed is active before it despawns naturally

    private Rigidbody _rb;
    void Start()
    {
        _rb = this.GetComponent<Rigidbody>();
    }

    void Update()
    {
        _rb.velocity = new Vector3(this._rb.velocity.x, this._rb.velocity.y, 15f);
        _lifetime -= Time.deltaTime;
        if(_lifetime <= 0)
        {
            Destroy(this.gameObject);
        }
    }


}
