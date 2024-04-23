using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static RangedMovement;

public class MeleeMovement : MonoBehaviour
{
    private Collider _playerCollider;
    private Vector3 _trackingPosition;
    private float _height;
    private bool _isIdle;
    private bool _isAtPlayer; // Exists for animation purposes
    public bool IsIdle => _isIdle;
    public bool IsAtPlayer => _isAtPlayer;  

    private Rigidbody _rigidBody;
    private DamageReceiver _damageReceiver;

    [Header("Player Detection")]
    [SerializeField, Tooltip("radius of sphere cast for obstruction detection")] private float _obstructionCheckRadius = .2f;
    [SerializeField, Tooltip("maximum number of obstructing objects detected in a single sphere cast")] private int _maxObstructions = 32;
    [SerializeField, Tooltip("layers considered for obstruction checks")] private LayerMask _obstructionLayers;
    [SerializeField, Tooltip("Range within which player causes enemy to enter attack mode")] private float _aggroRange = 50f;
    [SerializeField, Tooltip("Position of origin for the player detection sphere cast")] private Transform _spherecastOrigin;

    [Header("Movement Behavior")]
    [SerializeField, Tooltip("base move speed of ants")] private float _moveSpeed = 5f;
    [SerializeField, Tooltip("distance from target position at which ant stops moving")] private float _stoppingRange = 2f;

    [Header("Movement Smoothing")]
    [SerializeField, Tooltip("'snappiness' of rotating to goal position")] private float _rotationSharpness = 10f;
    [SerializeField, Tooltip("'snappiness' of velocity changes")] private float _velocitySharpness = 10f;
    [SerializeField, Tooltip("'snappiness' of coming to a stop when at target position")] private float _stoppingSharpness = 10f;
    [SerializeField, Tooltip("speed at which velocity is snapped back to zero")] private float _stoppingSpeedThreshold = 0.1f;

    [Header("Sound")]
    private AudioSource _audioSource;
    
    //Just movement sound
    [SerializeField] private List<AudioClip> _clips = new List<AudioClip>();
    private float _soundTimer = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        _playerCollider = GameObject.Find("Player").GetComponent<Collider>();
        _rigidBody = GetComponent<Rigidbody>();
        _damageReceiver = GetComponent<DamageReceiver>();
        _isIdle = true;
        _isAtPlayer = false;
        _trackingPosition = transform.position; // starts with no tracking
        _height = transform.position.y; // to prevent y value change
        _audioSource = GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    // Reference: https://www.youtube.com/watch?v=ZExSz7x69j8
    // https://discussions.unity.com/t/fastest-way-to-set-z-axis-rotation-to-0-c/220293
    void Update()
    {
        // lock ants to initial height (y) level
        transform.position = new Vector3(transform.position.x, _height, transform.position.z);

        // CHECK FOR NEW TRACKING POSITION (update only if player is visible, otherwise track to last known location)
        // Handle Obstructions
        RaycastHit closestHit = new();
        closestHit.distance = Mathf.Infinity; // collision distance (infinity by default = no collision)
        RaycastHit[] obstructions = new RaycastHit[_maxObstructions];
        int obstructionCount = Physics.SphereCastNonAlloc(_spherecastOrigin.position, _obstructionCheckRadius, 
            (_playerCollider.ClosestPoint(_spherecastOrigin.position) - _spherecastOrigin.position).normalized,
            obstructions, _aggroRange, _obstructionLayers, QueryTriggerInteraction.Ignore);
        // find closest obstruction
        for (int i = 0; i < obstructionCount; i++)
        {
            if (obstructions[i].distance < closestHit.distance && obstructions[i].distance > 0) closestHit = obstructions[i];
        }

        // exit idle mode if player detected with no obstructions
        if (closestHit.distance < Mathf.Infinity && closestHit.collider.CompareTag("Player"))
        {
            _trackingPosition = closestHit.point;
            _isIdle = false;
        }

        if(_damageReceiver.HealthLevel <= 0) // Ant should not be moving if dead
        {
            _isIdle = true;
        }

        if (!_isIdle)
        {   if(_soundTimer <= 0){
            _audioSource.PlayOneShot(_clips[0],GameManager.Instance.GetEnemyVolume());
            _soundTimer = 1.2f;
            }
            // Smoothly rotate to goal
            Quaternion goalRot = Quaternion.LookRotation(_trackingPosition - _spherecastOrigin.position, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, goalRot, 1f - Mathf.Exp(-_rotationSharpness * Time.deltaTime));

            // Move enemy towards player
            // If enemy is within 1 unit of player, stop moving
            if (Vector3.Distance(_trackingPosition, _spherecastOrigin.position) > _stoppingRange)
            {
                _isAtPlayer = false;
                // smoothly change velocity towards goal
                Vector3 goalVelocity = (_trackingPosition - _spherecastOrigin.position).normalized;
                goalVelocity.y = 0;
                goalVelocity *= _moveSpeed;
                _rigidBody.velocity = Vector3.Lerp(_rigidBody.velocity, goalVelocity, 1f - Mathf.Exp(-_velocitySharpness * Time.deltaTime));
            }
            else
            {
                // stop - within stopping range (with smoothing)
                _isAtPlayer = true;
                _rigidBody.velocity = Vector3.Lerp(_rigidBody.velocity, Vector3.zero, 1f - Mathf.Exp(-_stoppingSharpness * Time.deltaTime));
                if (_rigidBody.velocity.magnitude < _stoppingSpeedThreshold) _rigidBody.velocity = Vector3.zero;

                // re-enter idle if at target position but still no player visible
                if (Vector3.Distance(_spherecastOrigin.position, _playerCollider.ClosestPoint(_spherecastOrigin.position)) > _stoppingRange)
                {
                    _isIdle = true;
                    _isAtPlayer = false;
                }

            }
        }
        else{
            _rigidBody.velocity = Vector3.zero; // complete stop - idle
        }
        _soundTimer -= Time.deltaTime;
    }

    /// <summary>
    /// used for making ant move towards bullet that hit them if they are shot and they do not see player
    /// </summary>
    public void SetTrackingPositionIfIdle(Vector3 newPos)
    {
        if(_isIdle)
        {
            _trackingPosition = (newPos - _spherecastOrigin.position).normalized; // move slightly in direction of where bullet came from
            _isIdle = false;
        }
    }
}
