using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterState
{
    Moving,
    Stationary,
    Dash
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

    public CharacterState State { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();

        State = CharacterState.Stationary;
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
            case CharacterState.Moving:
                break;
            case CharacterState.Stationary:
                break;
            case CharacterState.Dash:
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
            case CharacterState.Moving:
                break;
            case CharacterState.Stationary:
                break;
            case CharacterState.Dash:
                break;
        }
    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        GatherInput();
        UpdateState();

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

    /// <summary>
    /// Sets player state based on inputs, rigidbody, or other data
    /// </summary>
    void UpdateState()
    {
        // Enter Moving (clarify where from - may be different for different states)
        if(PlayerInput.MoveAxisForward > 0f) // player holding input to move
        {
            TransitionToState(CharacterState.Moving);
        }

        // Any -> Stationary
        if(PlayerInput.MoveAxisForward == 0f && _rb.velocity.magnitude < _movingThreshold)
        {
            TransitionToState(CharacterState.Stationary);
        }

        // Any -> Stationary
        if(PlayerInput.DashDown)
        {
            TransitionToState(CharacterState.Dash);
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
        switch (State)
        {
            case CharacterState.Moving: // character tracks rotation to camera at a certain rate
                Quaternion m_planarCameraQuaternion = Quaternion.Euler(new Vector3(0f, _cameraTransform.rotation.eulerAngles.y, 0f));
                transform.rotation = Quaternion.Slerp(transform.rotation, m_planarCameraQuaternion, 1f - Mathf.Exp(-_movingRotationSharpness * Time.deltaTime));
                break;
            case CharacterState.Stationary: // character rotates faster tracking camera
                Quaternion s_planarCameraQuaternion = Quaternion.Euler(new Vector3(0f, _cameraTransform.rotation.eulerAngles.y, 0f));
                transform.rotation = Quaternion.Slerp(transform.rotation, s_planarCameraQuaternion, 1f - Mathf.Exp(-_stationaryRotationSharpness * Time.deltaTime));
                break;
            case CharacterState.Dash: // camera locked at current 'dashing' direction
                break;
        }
    }
    #endregion

    #region UPDATE-VELOCITY
    [Header("Velocity")]
    [SerializeField, Tooltip("strength of force applied to make character move forwards")] private float _movementForce = 5f;
    [SerializeField, Tooltip("terminal speed that character is capped at")] private float _maxSpeed = 5f;

    /// <summary>
    /// Update player velocity based on inputs.
    /// After rotation update since velocity should only be 'forwards' (worm physics)
    /// </summary>
    void UpdateVelocity()
    {
        switch (State)
        {
            case CharacterState.Moving: // moves forwards (in direction of player facing) only

                // apply moving force
                _rb.AddForce(PlayerInput.MoveAxisForward * transform.forward * _movementForce * Time.deltaTime);
                // check for max speed
                if (_rb.velocity.magnitude > _maxSpeed) _rb.velocity = _rb.velocity.normalized * _maxSpeed;

                break;
            case CharacterState.Stationary: // no velocity (hence, stationary)
                break;
            case CharacterState.Dash: // fixed velocity in fixed direction
                break;
        }
    }
    #endregion
}
