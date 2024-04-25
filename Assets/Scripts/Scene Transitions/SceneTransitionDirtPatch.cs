using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            // make sure it is an enemy and NOT a dummy target
            if (obj.CompareTag("Enemy") && (obj.TryGetComponent(out RangedMovement compWasp) || (obj.TryGetComponent(out MeleeMovement compAnt))))
            {
                enemiesNearby = true;
                break;
            }
        }

        // enemies can be nearby in maze room to avoid issues if enemies are on other side of wall
        if (CanMoveOn && (!enemiesNearby || SceneManager.GetActiveScene().name == "6_MazeRoom"))
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
