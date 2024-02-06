using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.tag = "bullet";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
