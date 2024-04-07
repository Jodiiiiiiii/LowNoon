using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableComponentOnStart : MonoBehaviour
{
    [SerializeField, Tooltip("The component to be enabled upon starting play mode / loading the scene")] private Behaviour _component;

    // Start is called before the first frame update
    void Start()
    {
        _component.enabled = true;
    }
}
