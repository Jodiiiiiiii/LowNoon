using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class exists to prevent tubmleweeds from moving through our town buildings.
// That's, uh, kind of it.
public class TumbleweedDestroyer : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Tumbleweed"))
        {
            Destroy(collision.gameObject);
        }
    }
}
