using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class RangedMovement : MonoBehaviour
{
    public enum RangeEnemyMoveState
    {
        ZIPPY,
        STILL,
        AWAY,
        TOWARDS,
        IDLE,
    }

    [Header("Move State")]
    public RangeEnemyMoveState EnemyMoveState;
    public bool StayStill;

    private GameObject _player;
    private Vector3 _playerPosition;

    [Header("Speeds")]
    public float MoveSpeed;
    public float ZipSpeed;
    public float HoverSpeed;
    public float VelocitySharpness;

    [Header("Enemy Distance")]
    public float MinDistFromPlayer;
    public float MaxDistFromPlayer;
    public float MinDistRandom;
    public float MaxDistRandom;
    private float _distVar;

    [Header("Hover")]
    public float MinHoverFromGround;
    public float MaxHoverFromGround;
    public float MinHoverRandom;
    public float MaxHoverRandom;
    private int _hoverDirection;
    private float _hoverVar;
    [SerializeField, Tooltip("factor by which hovering speed increases while in zippy state")] private float _zippyHoverFactor;

    [Header("Timers")]
    public float ZippyTimer;
    public float StillTimer;
    private float _timer;

    [Header("Timer Modifiers")]
    public float ZippyTimerMin;
    public float ZippyTimerMax;
    public float StillTimerMin;
    public float StillTimerMax;
    private float _zippyVar;
    private float _stillVar;

    [Header("Player Detection")]
    [SerializeField, Tooltip("radius of sphere cast for obstruction detection")] private float _obstructionCheckRadius = 2f;
    [SerializeField, Tooltip("maximum number of obstructing objects detected in a single sphere cast")] private int _maxObstructions = 32;
    [SerializeField, Tooltip("layers considered for obstruction checks")] private LayerMask _obstructionLayers;
    [SerializeField, Tooltip("Range within which player causes enemy to enter attack mode")] private float _aggroRange = 50f;


    private Rigidbody _rigidBody;
    private int _leftOrRight;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        _rigidBody = GetComponent<Rigidbody>();

        _leftOrRight = ChangeDirection();

        _hoverDirection = Random.Range(0, 2);
        _hoverDirection = (_hoverDirection == 0) ? -1 : 1;

        _distVar = Random.Range(MinDistRandom, MaxDistRandom);
        _hoverVar = ChangeMaxHover();

        Vector3 startPos = transform.position;
        startPos.y = Random.Range(MinHoverFromGround, MaxHoverFromGround);
        transform.position = startPos;
        _timer = 0;

        _zippyVar = ZippyRando();
        _stillVar = StillRando();

        EnemyMoveState = RangeEnemyMoveState.IDLE;
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer > StillTimer + _stillVar && StayStill)
        {
            _timer = 0;
            StayStill = false;
            _zippyVar = ZippyRando();
            _leftOrRight = ChangeDirection();
        }
        else if (_timer > ZippyTimer + _zippyVar && !StayStill)
        {
            _timer = 0;
            StayStill = true;
            _stillVar = StillRando();
        }
        _playerPosition = _player.transform.position;
        

        // The enemies will first try to move away from the enemy 
        // Ranged enemies will spawn with a random direction either left or right
        // They will circle around the player in the direction they spawned with
        if (EnemyMoveState != RangeEnemyMoveState.IDLE)
        {
            // Set enemy rotation to face player
            transform.LookAt(_playerPosition);
            Vector3 eulerAngles = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(0, eulerAngles.y, 0);

            if (Vector3.Distance(_playerPosition, transform.position) < MinDistFromPlayer + _distVar)
            {
                EnemyMoveState = RangeEnemyMoveState.AWAY;
            }
            else if (Vector3.Distance(_playerPosition, transform.position) > MaxDistFromPlayer + _distVar)
            {
                EnemyMoveState = RangeEnemyMoveState.TOWARDS;
            }
            else if (StayStill)
            {
                EnemyMoveState = RangeEnemyMoveState.STILL;
            }
            else
            {
                EnemyMoveState = RangeEnemyMoveState.ZIPPY;
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
                EnemyMoveState = RangeEnemyMoveState.STILL;
        }

        Vector3 direction = Vector3.zero; // stays zero if in still state
        if (EnemyMoveState == RangeEnemyMoveState.AWAY)
        {
            direction = (-transform.forward + transform.right * _leftOrRight).normalized * MoveSpeed;
        }
        else if (EnemyMoveState == RangeEnemyMoveState.TOWARDS)
        {
            direction = (transform.forward + transform.right * _leftOrRight).normalized * MoveSpeed;
        }
        else if (EnemyMoveState == RangeEnemyMoveState.ZIPPY)
        {
            direction = (transform.right * _leftOrRight).normalized * ZipSpeed;
        }

        if (transform.position.y > MaxHoverFromGround + _hoverVar)
        {
            _hoverDirection = -1;
        }
        else if (transform.position.y < MinHoverFromGround)
        {
            _hoverDirection = 1;
            _hoverVar = ChangeMaxHover();
        }

        direction.y = _hoverDirection * HoverSpeed * (EnemyMoveState == RangeEnemyMoveState.ZIPPY ? _zippyHoverFactor : 1);
        _rigidBody.velocity = Vector3.Lerp(_rigidBody.velocity, direction, 1f - Mathf.Exp(-VelocitySharpness * Time.deltaTime));
    }

    float ChangeMaxHover()
    {
        return _hoverVar = Random.Range(MinHoverRandom, MaxHoverRandom); ;
    }

    int ChangeDirection()
    {
        int temp = Random.Range(0, 2);
        temp = (temp == 0) ? -1 : 1;
        return temp;
    }

    float ZippyRando()
    {
        return (Random.Range(ZippyTimerMin, ZippyTimerMax));
    }

    float StillRando()
    {
        return Random.Range(StillTimerMin, StillTimerMax);
    }

    // TODO
    // X Make max hover vary each time it floats up 
    // X Make range variables public
    // X Make random ranges public
    // - Make left and right movement more jittery
    // - - Have a timer for wait time and move time
    // - -  Make enemies change directions when they start moving again
}
