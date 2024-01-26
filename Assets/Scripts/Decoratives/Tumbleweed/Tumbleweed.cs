using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class controls the behavior of a tumbleweed, namely its speed and lifetime.

public class Tumbleweed : MonoBehaviour
{
    [SerializeField] private float _speed = 15f; // How fast the tumbleweed is going
    [SerializeField] private float _lifetime = 45f;  // How long the tumbleweed is active before it despawns naturally

    private Rigidbody _rb;
    void Start()
    {
        _rb = this.GetComponent<Rigidbody>();
        Invoke("DestroyMe", _lifetime);
    }

    void Update()
    {
        _rb.velocity = new Vector3(this._rb.velocity.x, this._rb.velocity.y, _speed);
    }

    private void DestroyMe()
    {
        Destroy(this.gameObject);
    }


}
