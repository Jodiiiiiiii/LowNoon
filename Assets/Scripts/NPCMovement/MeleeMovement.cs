using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static RangedMovement;

public class MeleeMovement : MonoBehaviour
{
    private GameObject _player;
    private Vector3 _playerPosition;
    private bool _isIdle;

    public float MoveSpeed = 5f;

    private Rigidbody _rigidBody;

    [Header("Player Detection")]
    [SerializeField, Tooltip("radius of sphere cast for obstruction detection")] private float _obstructionCheckRadius = .2f;
    [SerializeField, Tooltip("maximum number of obstructing objects detected in a single sphere cast")] private int _maxObstructions = 32;
    [SerializeField, Tooltip("layers considered for obstruction checks")] private LayerMask _obstructionLayers;
    [SerializeField, Tooltip("Range within which player causes enemy to enter attack mode")] private float _aggroRange = 50f;


    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        _rigidBody = GetComponent<Rigidbody>();
        _isIdle = true;
    }

    // Update is called once per frame
    // Reference: https://www.youtube.com/watch?v=ZExSz7x69j8
    // https://discussions.unity.com/t/fastest-way-to-set-z-axis-rotation-to-0-c/220293
    void Update()
    {
        _playerPosition = _player.transform.position;

        if (!_isIdle)
        {
            // Set enemy rotation to face player
            transform.LookAt(_playerPosition);
            Vector3 eulerAngles = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(0, eulerAngles.y, 0);


            // Move enemy towards player
            // If enemy is within 1 unit of player, stop moving
            if (Vector3.Distance(_playerPosition, transform.position) > 2f)
            {
                Vector3 direction = (_playerPosition - transform.position).normalized * MoveSpeed;
                direction.y = _rigidBody.velocity.y;
                _rigidBody.velocity = direction;
            }
            else
            {
                _rigidBody.velocity = new Vector3(0, _rigidBody.velocity.y, 0);
            }
        }
        else
        {
            // Handle Obstructions
            RaycastHit closestHit = new();
            closestHit.distance = Mathf.Infinity; // collision distance (infinity by default = no collision)
            RaycastHit[] obstructions = new RaycastHit[_maxObstructions];
            int obstructionCount = Physics.SphereCastNonAlloc(transform.position, _obstructionCheckRadius, (_playerPosition - transform.position).normalized,
                obstructions, _aggroRange, _obstructionLayers, QueryTriggerInteraction.Ignore);
            // find closest obstruction
            for (int i = 0; i < obstructionCount; i++)
            {
                if (obstructions[i].distance < closestHit.distance && obstructions[i].distance > 0) closestHit = obstructions[i];
            }

            // exit idle mode if player detected with no obstructions
            if (closestHit.distance < Mathf.Infinity && closestHit.collider.CompareTag("Player"))
                _isIdle = false;
        }
        
    }
}
