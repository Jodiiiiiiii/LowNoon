using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// handles inputs for player shooting, determines if it is a valid time to shoot, and creates bullets
/// </summary>
public class PlayerShooting : MonoBehaviour
{
    [Header("Bullet")]
    [Tooltip("stores prefab of bullet object")] public GameObject BulletObject;

    [Header("Trajectory")]
    [SerializeField, Tooltip("origin of bullets being fired")] private Transform _gunPosition;
    [SerializeField, Tooltip("angle added to camera angle to determine final trajectory")] private float _fireAngle = 30f;
    [SerializeField, Tooltip("max ranged of the shootcast from the camera to detect what the target position is")] private float _maxShootCastRange = 100f;
    
    private PlayerController _playerController;

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
            // create new bullet
            GameObject newBullet = Instantiate(BulletObject);

            // set position of new bullet
            newBullet.transform.position = _gunPosition.position;

            Vector3 projectileDirection;

            // Calculate planar angle offset of player from camera
            float playerAngleOffset = Mathf.Abs(Camera.main.transform.rotation.eulerAngles.y - _playerController.transform.rotation.eulerAngles.y);
            if (playerAngleOffset > 180) playerAngleOffset = 360 - playerAngleOffset;
            if (Camera.main.transform.rotation.eulerAngles.y - _playerController.transform.rotation.eulerAngles.y > 0) playerAngleOffset *= -1;

            // calculate shoot direction based on 
            Vector3 shootDir = Quaternion.AngleAxis(playerAngleOffset, Vector3.up) * Camera.main.transform.forward;

            // raycast to find projectile direction (actual trajectory) from shoot direction (raycast from camera)
            Ray shootRay = new(Camera.main.transform.position, shootDir);
            if(Physics.Raycast(shootRay, out RaycastHit hitInfo))
            {
                projectileDirection = (hitInfo.point - _gunPosition.position).normalized;
            }
            else
            {
                projectileDirection = (shootRay.GetPoint(_maxShootCastRange) - _gunPosition.position).normalized;
            }

            Debug.DrawRay(_gunPosition.position, projectileDirection);

            newBullet.GetComponent<Rigidbody>().AddForce(newBullet.GetComponent<BulletStats>().InitialForce * projectileDirection, ForceMode.Impulse);

            _timer = 0.0f; // reset cooldown timer
        }

        _timer += Time.deltaTime;
    }
}
