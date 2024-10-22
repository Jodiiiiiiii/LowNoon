using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualCameraController : MonoBehaviour
{

    public bool activeCoroutine;
    private CameraController _cameraController;
    private Transform _playerTransform;
    private Camera _camera;
    
    Vector3 _openingLogoPosition;
    Vector3 _startPosition = new Vector3(12.79f, 2f, -71f);

    // Scripted transform positions (or transform values relative to the player) for certain scenarios
    Vector3 _mainMenuPosition = new Vector3(-6.33f, .68f, 2.05f);
    Vector3 _playerCameraPosition = new Vector3(1.7f, 4.015f, -10f);
    Vector3 _revivePosition = new Vector3(-30.316f, 3.2999f, -33.238f);


    // Scripted rotational positions for certain scenarios
    private Vector3 _openingLogoRotation;                               // Rotation for the opening shot of the logo
    private Vector3 _mainMenuRotation = new Vector3(-13.925f, 90, 0);   // Rotation in main menu
    private Vector3 _playerCameraRotation = Vector3.zero;                              // Rotation to travel to before giving the player control
    private Vector3 _reviveRotation = new Vector3(0f, 54.997f, 0);

    [SerializeField, Tooltip("Distance from targetPos at which camera will stop smoothing and snap to goal")] private float _cameraSmoothingThreshold = 0.05f;

    void Start()
    {
        _cameraController = GetComponent<CameraController>();
        _playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        _mainMenuPosition = _playerTransform.position + _mainMenuPosition;
        _playerCameraPosition = _playerTransform.position + _playerCameraPosition;
        _camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void moveToMainMenu()
    {
        StopAllCoroutines();
        StartCoroutine(DoCamPosition(_mainMenuPosition, 2f, _mainMenuRotation));
    }

    public void moveToGameStart()
    {
        StopAllCoroutines();
        StartCoroutine(DoCamPosition(_playerCameraPosition, 1f, _playerCameraRotation));
    }

    public void moveToReviveStart()
    {
        StopAllCoroutines();
        StartCoroutine(DoCamPosition(_revivePosition, .5f, _reviveRotation));
    }

    public void moveToGivenPos(Vector3 pos, Vector3 rot, float speed)
    {
        StopAllCoroutines();
        StartCoroutine(DoCamPosition(pos, speed, rot));
    }

    public void lerpToFOV(float FOV, float time)
    {
        StartCoroutine(DoCamFOV(FOV, time));
    }

    // The camera movement coroutine that all of the bespoke camera movements use
    IEnumerator DoCamPosition(Vector3 targetPos, float travelTime, Vector3 targetRotation)
    {
        activeCoroutine = true;

        float xAngle;
        float yAngle;
        float zAngle;

        Vector3 velocity = Vector3.zero;    // Initial velocity values for the damping functions
        float xVelocity = 0f;
        float yVelocity = 0f;
        float zVelocity = 0f;

        while (Vector3.Distance(transform.position, targetPos) >= _cameraSmoothingThreshold)
        {
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, travelTime); // Move camera position

            xAngle = Mathf.SmoothDampAngle(transform.eulerAngles.x, targetRotation.x, ref xVelocity, travelTime);
            yAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation.y, ref yVelocity, travelTime);
            zAngle = Mathf.SmoothDampAngle(transform.eulerAngles.z, targetRotation.z, ref zVelocity, travelTime);

            transform.eulerAngles = new Vector3(xAngle, yAngle, zAngle);    // Change camera rotation
            yield return null;
        }
        transform.position = targetPos; // snap to goal value
        activeCoroutine = false;
        yield return null;
    }

    private IEnumerator DoCamFOV(float targetFOV, float travelTime)
    {
        float velocity = 0 ;
        while (Mathf.Abs(targetFOV - _camera.fieldOfView) >= _cameraSmoothingThreshold)
        {
            _camera.fieldOfView = Mathf.SmoothDamp(_camera.fieldOfView, targetFOV, ref velocity, travelTime); // Move camera position

            yield return null;
        }

        _camera.fieldOfView = targetFOV;
        yield return null;
    }
}
