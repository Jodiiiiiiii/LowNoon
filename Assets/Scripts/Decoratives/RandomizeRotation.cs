using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeRotation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vector3 rot = transform.rotation.eulerAngles;
        rot.y = Random.Range(0, 180);
        transform.rotation = Quaternion.Euler(rot);
    }
}
