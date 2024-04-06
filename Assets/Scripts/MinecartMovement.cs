using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinecartMovement : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float _speed = 50.0f;
    [SerializeField] private GameObject _explosionParticles;
    [SerializeField] private float _lifetime = 5.0f;

    public float ExplosionRadius;
    public int DamageAmount;
    private Rigidbody _rb;
    
    void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody>();
        _rb.velocity = transform.forward * _speed;
        Invoke("DestroyMe", _lifetime);
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    void OnCollisionEnter(Collision collision){
        if(collision.collider.tag == "Player" || collision.collider.tag == "Enemy"){
            Instantiate(_explosionParticles, gameObject.transform.position, gameObject.transform.rotation);
            Collider[] damagedObjects = Physics.OverlapSphere(transform.position, ExplosionRadius);
                foreach (var obj in damagedObjects)
                {
                    if(obj.tag == "Player"){
                        obj.GetComponent<PlayerHealth>().ExplosionDmg = DamageAmount;
                    }
                    else if(obj.GetComponent<DamageReceiver>() != null){
                        //Debug.Log("enemy");
                        // question mark question mark question mark
                        DamageReceiver healthLevel = obj.GetComponent<DamageReceiver>();
                        healthLevel.HealthLevel -= (float)DamageAmount;
                        //Debug.Log("Other health: " + healthLevel.HealthLevel);

                    }
                }
                
            Destroy(gameObject);
        }
    }

    void DestroyMe(){
        Destroy(this.gameObject);
    }
    

}
