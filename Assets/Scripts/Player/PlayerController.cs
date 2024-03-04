using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterState
{
    MOVING,
    STATIONARY,
    DASH
}

public class PlayerCharacterInputs
{
    public float MoveAxisForward;
    public bool DashDown;
    public float LookAxisRight;
    public float LookAxisUp;
}

public class PlayerController : MonoBehaviour
{
    // Components
    private Rigidbody _rb;

    public CharacterState State { get; private set; } // tracks the players current state; likely useful for animator

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();

        State = CharacterState.STATIONARY;
    }

    #region CHARACTER-STATES
    /// <summary>
    /// Handles movement state transitions and enter/exit callbacks
    /// </summary>
    public void TransitionToState(CharacterState newState)
    {
        CharacterState tmpInitialState = State;
        OnStateExit(tmpInitialState, newState);
        State = newState;
        OnStateEnter(newState, tmpInitialState);
    }

    /// <summary>
    /// Events when entering a state
    /// </summary>
    /// <param name="state">state being entered</param>
    /// <param name="fromState">state being exited</param>
    public void OnStateEnter(CharacterState state, CharacterState fromState)
    {
        switch(state)
        {
            case CharacterState.MOVING:
                break;
            case CharacterState.STATIONARY:
                break;
            case CharacterState.DASH:
                break;
        }
    }

    /// <summary>
    /// Events when exiting a state
    /// </summary>
    /// <param name="state">state being exited</param>
    /// <param name="toState">state being entered</param>
    public void OnStateExit(CharacterState state, CharacterState toState)
    {
        switch (state)
        {
            case CharacterState.MOVING:
                break;
            case CharacterState.STATIONARY:
                break;
            case CharacterState.DASH:
                break;
        }
    }
    #endregion

    private void Awake()
    {
        // Moved cursor lock to in game UI view
    }

    // Update is called once per frame
    void Update()
    {
        GatherInput();
        UpdateState();
    }

    private void FixedUpdate()
    {
        UpdateRotation();
        UpdateVelocity();
    }

    #region GATHER-INPUT
    // input constants
    private const string VERTICAL_INPUT = "Vertical";
    private const string MOUSE_RIGHT = "Mouse X";
    private const string MOUSE_UP = "Mouse Y";

    public PlayerCharacterInputs PlayerInput { get; private set; } = new PlayerCharacterInputs();

    /// <summary>
    /// Updates components of PlayerInput based on Unity Input methods
    /// </summary>
    void GatherInput()
    {
        // Character inputs
        PlayerInput.MoveAxisForward = Input.GetAxisRaw(VERTICAL_INPUT);
        PlayerInput.DashDown = Input.GetKeyDown(KeyCode.LeftShift);

        // Camera inputs
        PlayerInput.LookAxisRight = Input.GetAxisRaw(MOUSE_RIGHT);
        PlayerInput.LookAxisUp = Input.GetAxisRaw(MOUSE_UP);
    }
    #endregion

    #region UPDATE-STATE
    [Header("Player State")]
    [SerializeField, Tooltip("speed which player must be below to be considered 'stationary'")] private float _movingThreshold = 0.01f;

    [Header("Dashing")]
    [SerializeField, Tooltip("Cooldown for activating player dash")] private float _dashCooldown = 5f;
    [SerializeField, Tooltip("strength of force applied to make character dash forwards")] private float _dashForce = 8f;
    [SerializeField, Tooltip("terminal dash speed that character is capped at")] private float _maxDashSpeed = 15f;
    [SerializeField, Tooltip("time from starting dash when dashing will stop")] private float _dashDuration = 1.5f;

    private float _dashTimer = 0f; // used both for cooldown (when not dashing) and dash duration (when dashing)

    /// <summary>
    /// Sets player state based on inputs, rigidbody, or other data
    /// </summary>
    void UpdateState()
    {
        // DASH state
        if(State == CharacterState.DASH)
        {
            // DASH -> STATIONARY
            if (_dashTimer <= 0 && _rb.velocity.magnitude < _movingThreshold) // dash has hit max duration and player has come to a stop
            {
                _dashTimer = _dashCooldown; // start dash cooldown cooldown timer
                TransitionToState(CharacterState.STATIONARY);

            }
            else // update duration timer
                _dashTimer -= Time.deltaTime;
        }
        else // NOT DASH state
        {
            // Not DASH -> DASH
            if (PlayerInput.DashDown && _dashTimer <= 0f)
            {
                _dashTimer = _dashDuration; // start dash duration timer
                TransitionToState(CharacterState.DASH);
            }
            else // update dash cooldown timer + handle other states
            {
                _dashTimer -= Time.deltaTime;

                // Not DASH -> MOVING
                if (PlayerInput.MoveAxisForward > 0f) // player holding input to move
                {
                    TransitionToState(CharacterState.MOVING);
                }

                //Debug.Log(_rb.velocity.magnitude); Fluctuates between .58 and .93 after tapping W
                // Not DASH -> STATIONARY
                if (PlayerInput.MoveAxisForward <= 0f && _rb.velocity.magnitude < _movingThreshold)
                {
                    TransitionToState(CharacterState.STATIONARY);
                }
            }
        }
    }
    #endregion

    #region UPDATE-ROTATION
    [Header("Rotation")]
    [SerializeField, Tooltip("used for rotating player towards camera angle")] private Transform _cameraTransform;
    [SerializeField, Tooltip("'snappiness' of character seeking camera angle while moving")] private float _movingRotationSharpness = 1f;
    [SerializeField, Tooltip("'snappiness' of character seeking camera angle while stationary")] private float _stationaryRotationSharpness = 2f;
    
    /// <summary>
    /// Update player velocity based on inputs
    /// </summary>
    void UpdateRotation()
    {
        // planar rotation (world space) - aligns player transform with camera planar angle
        Quaternion planarCameraQuaternion = Quaternion.Euler(new Vector3(0f, _cameraTransform.rotation.eulerAngles.y, 0f));

        switch (State)
        {
            case CharacterState.MOVING: // character tracks rotation to camera at a certain rate
                // smoothing planar rotation
                // turning speed also scales with move speed stat
                _rb.MoveRotation(Quaternion.Slerp(transform.rotation, planarCameraQuaternion,
                    1f - Mathf.Exp(-_movingRotationSharpness * Time.deltaTime * GameManager.Instance.PlayerData.MoveSpeed)));

                break;
            case CharacterState.STATIONARY: // character rotates faster tracking camera
                // smooth planar rotation
                // speed also scales with move speed stat
                _rb.MoveRotation(Quaternion.Slerp(transform.rotation, planarCameraQuaternion, 
                    1f - Mathf.Exp(-_stationaryRotationSharpness * Time.deltaTime * GameManager.Instance.PlayerData.MoveSpeed)));

                break;
            case CharacterState.DASH: // camera locked at current 'dashing' direction
                // no rotation change while dashing
                break;
        }
    }
    #endregion

    #region UPDATE-VELOCITY
    [Header("Movement")]
    [SerializeField, Tooltip("strength of force applied to make character move forwards")] private float _moveForce = 5f;
    [SerializeField, Tooltip("terminal move speed that character is capped at")] private float _maxMoveSpeed = 5f;
    [SerializeField, Tooltip("strength of force applied by friction; no distinction between static/dynamic")] private float _frictionForce = 2f;

    /// <summary>
    /// Update player velocity based on inputs.
    /// After rotation update since velocity should only be 'forwards' (worm physics)
    /// </summary>
    void UpdateVelocity()
    {
        switch (State)
        {
            case CharacterState.MOVING: // moves forwards (in direction of player facing) only

                // apply moving force
                // move speed force scales with move speed stat
                _rb.AddForce(PlayerInput.MoveAxisForward * transform.forward * _moveForce * GameManager.Instance.PlayerData.MoveSpeed);

                // apply backwards friction
                _rb.AddForce(-_rb.velocity.normalized * _frictionForce);
                
                // scales max move speed with move speed stat
                float actualMaxMoveSpeed = _maxMoveSpeed * GameManager.Instance.PlayerData.MoveSpeed;
                // check for max move speed
                if (_rb.velocity.magnitude > actualMaxMoveSpeed)
                    _rb.velocity = _rb.velocity.normalized * actualMaxMoveSpeed;
           
                break;
            case CharacterState.STATIONARY:
                // no change
                _rb.velocity = Vector3.zero; // ensure a complete stop after crossing threshold speed for stationary

                break;
            case CharacterState.DASH: // fixed velocity in fixed direction
                
                // apply dashing force (only if dash duration hasn't expired)
                if(_dashTimer > 0)
                    // dash speed scales with move speed stat
                    _rb.AddForce(transform.forward * _dashForce * GameManager.Instance.PlayerData.MoveSpeed);
                else
                    // apply backwards friction - only if not still adding force
                    _rb.AddForce(-_rb.velocity.normalized * _frictionForce);

                // scales max dash speed with move speed stat
                float actualMaxDashSpeed = _maxDashSpeed * GameManager.Instance.PlayerData.MoveSpeed;
                // check for max dash speed
                if (_rb.velocity.magnitude > actualMaxDashSpeed)
                    _rb.velocity = _rb.velocity.normalized * actualMaxDashSpeed;

                break;
        }
    }
    #endregion
}
