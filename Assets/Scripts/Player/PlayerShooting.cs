using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// handles inputs for player shooting, determines if it is a valid time to shoot, and creates bullets
/// </summary>
public class PlayerShooting : MonoBehaviour
{

    [Tooltip("stores prefab of bullet object")] public GameObject BulletObject;

    PlayerController _playerController;

    float _timer;

    // Start is called before the first frame update
    void Start()
    {
        _playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        _timer = GameManager.Instance.PlayerData.BulletCooldown; // start with no cooldown
    }

    // Update is called once per frame
    void Update()
    {
        // shooting input AND not during cooldown time AND in stationary state
        if (Input.GetKeyDown(KeyCode.Mouse0) && _timer > GameManager.Instance.PlayerData.BulletCooldown && _playerController.State == CharacterState.STATIONARY)
        {
            GameObject newBullet = Instantiate(BulletObject);

            // TODO: add configurable parameters so that bullet spawns coming out of gun
            newBullet.transform.position = transform.position + (0.5f * transform.forward); // temporary
            newBullet.GetComponent<Rigidbody>().AddForce(newBullet.GetComponent<BulletStats>().InitialForce * transform.forward, ForceMode.Impulse);

            _timer = 0.0f;
        }

        _timer += Time.deltaTime;
    }
}
