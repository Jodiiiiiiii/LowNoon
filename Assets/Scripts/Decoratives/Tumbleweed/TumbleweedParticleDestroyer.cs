using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TumbleweedParticleDestroyer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyMe", .5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void DestroyMe()
    {
        Destroy(this.gameObject);
    }
}
