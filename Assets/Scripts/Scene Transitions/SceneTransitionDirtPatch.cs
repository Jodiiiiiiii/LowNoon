using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionDirtPatch : MonoBehaviour
{

    [SerializeField] Collider _transitionCollider;
    [SerializeField] GameObject _glowParticles;

    [SerializeField, Tooltip("Range within which there must be no enemies for patch to be usable")] private float _enemyCheckRadius = 5f;

    public bool CanMoveOn; // When we "clear" a scene, just toggle this
    void Start()
    {
        _transitionCollider.enabled = false;
        _glowParticles.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // check for nearby enemies
        bool enemiesNearby = false;
        Collider[] nearbyObjs = Physics.OverlapSphere(transform.position, _enemyCheckRadius);
        foreach (var obj in nearbyObjs)
        {
            if (obj.CompareTag("Enemy"))
            {
                enemiesNearby = true;
                break;
            }
        }

        if (CanMoveOn && !enemiesNearby)
        {
            _transitionCollider.enabled = true;
            _glowParticles.SetActive(true);
        }
        else // allows deactivating if an enemy comes too close into range
        {
            _transitionCollider.enabled = false;
            _glowParticles.SetActive(false);
        }
    }
}
