using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class controls the behavior of a tumbleweed, namely its speed and lifetime.

public class Tumbleweed : MonoBehaviour
{
    public float Speed = 15f; // How fast the tumbleweed is going
    [SerializeField] private float _lifetime = 45f;  // How long the tumbleweed is active before it despawns naturally
    [SerializeField] private GameObject deathParticles;

    private DamageReceiver _receiver;
    private AudioSource _audioSource;

    private Rigidbody _rb;
    void Start()
    {
        _rb = this.GetComponent<Rigidbody>();
        _receiver = this.GetComponent<DamageReceiver>();
        _audioSource = this.GetComponent<AudioSource>();
        Invoke("DestroyMe", _lifetime);
       // InvokeRepeating("PlaySound", .9167f, 2f);
       // InvokeRepeating("PlaySound", 2f, 2f);
    }

    void Update()
    {
        _rb.velocity = transform.forward * Speed;
        if(_receiver.HealthLevel <= 0)
        {
            Instantiate(deathParticles, this.transform.position, this.transform.rotation);
            Destroy(this.gameObject);
        }
    }

    private void DestroyMe()
    {
        Destroy(this.gameObject);
    }

    private void PlaySound()
    {
        _audioSource.Play();
    }


}
