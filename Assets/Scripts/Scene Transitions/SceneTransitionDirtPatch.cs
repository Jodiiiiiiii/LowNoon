using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionDirtPatch : MonoBehaviour
{

    [SerializeField] GameObject _transitionCollider;
    [SerializeField] GameObject _glowParticles;

    public bool CanMoveOn; // When we "clear" a scene, just toggle this
    void Start()
    {
        _transitionCollider.SetActive(false);
        _glowParticles.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (CanMoveOn)
        {
            _transitionCollider.SetActive(true);
            _glowParticles.SetActive(true);
        }
    }
}
