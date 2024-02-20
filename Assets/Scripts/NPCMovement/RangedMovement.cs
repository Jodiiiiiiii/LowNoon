using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class RangedMovement : MonoBehaviour
{
    public enum RangeEnemyMoveState
    {
        ZIPPY,
        STILL,
        AWAY,
        TOWARDS,
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


    private Rigidbody _rigidBody;
    private int _leftOrRight;

    // Start is called before the first frame update
    void Start()
    {
        _player = _player = GameObject.FindWithTag("Player");
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
        }
        else if (_timer > ZippyTimer + _zippyVar && !StayStill)
        {
            _timer = 0;
            StayStill = true;
            _stillVar = StillRando();
        }
        _playerPosition = _player.transform.position;

        // Set enemy rotation to face player
        transform.LookAt(_playerPosition);
        Vector3 eulerAngles = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0, eulerAngles.y, 0);

        // The enemies will first try to move away from the enemy 
        // Ranged enemies will spawn with a random direction either left or right
        // They will circle around the player in the direction they spawned with
        if (Vector3.Distance(_playerPosition, transform.position) < MinDistFromPlayer + _distVar)
        {
            EnemyMoveState = RangeEnemyMoveState.AWAY;
        }
        else if (Vector3.Distance(_playerPosition, transform.position) > MaxDistFromPlayer + _distVar)
        {
            EnemyMoveState= RangeEnemyMoveState.TOWARDS;
        }
        else if (StayStill)
        {
            EnemyMoveState = RangeEnemyMoveState.STILL;
        }
        else
        {
            EnemyMoveState = RangeEnemyMoveState.ZIPPY;
        }

        Vector3 direction;
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
        else
        {
            direction.x = 0;
            direction.z = 0;
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

        direction.y = _hoverDirection * transform.up.y * HoverSpeed;
        _rigidBody.velocity = direction;

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
