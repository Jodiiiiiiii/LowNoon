using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWithAnyChild : MonoBehaviour
{
    [SerializeField, Tooltip("Number of child objects required - or else this object is destroyed")] private int _numChildren = 3;

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.childCount < _numChildren)
            Destroy(gameObject);
    }
}
