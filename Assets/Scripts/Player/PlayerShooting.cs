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
    
    private CameraController _cameraController;
    private PlayerController _playerController;

    float _timer;

    // Start is called before the first frame update
    void Start()
    {
        _playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        _cameraController = Camera.main.GetComponent<CameraController>();

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
            newBullet.transform.position = _gunPosition.position;

            // rotate player transform to camera angle
            //Vector3 shootDirection = _playerController.transform.forward;
            //Quaternion rotation = Quaternion.AngleAxis(_cameraController.TargetVertAngle - _fireAngle, Vector3.Cross(Vector3.up, _playerController.transform.forward));
            //shootDirection = rotation * shootDirection;

            RaycastHit hitInfo;
            Vector3 projectileDirection;

            Ray shootDirection = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            if(Physics.Raycast(shootDirection, out hitInfo))
            {
                projectileDirection = (hitInfo.point - _gunPosition.position).normalized;
            }
            else
            {
                projectileDirection = (shootDirection.GetPoint(_maxShootCastRange) - _gunPosition.position).normalized;
            }

            Debug.DrawRay(_gunPosition.position, projectileDirection);

            newBullet.GetComponent<Rigidbody>().AddForce(newBullet.GetComponent<BulletStats>().InitialForce * projectileDirection, ForceMode.Impulse);

            _timer = 0.0f; // reset cooldown timer
        }

        _timer += Time.deltaTime;
    }
}
