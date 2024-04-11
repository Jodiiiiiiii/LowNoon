using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ensures only one upgrade option from an upgrade barrel can be picked up
/// </summary>
public class UpgradeBarrelPickups : MonoBehaviour
{
    [SerializeField, Tooltip("Number of child objects required - or else this object is destroyed")] private int _numChildren = 3;

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.childCount < _numChildren)
        {
            GameObject.Find("Exit").GetComponent<SceneTransitionDirtPatch>().CanMoveOn = true; // activate dirt patch
            Destroy(gameObject);
        }
    }
}
