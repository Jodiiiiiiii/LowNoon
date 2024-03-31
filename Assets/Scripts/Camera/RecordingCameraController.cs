using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordingCameraController : MonoBehaviour
{
    private bool activeCoroutine;

    [SerializeField, Tooltip("Approximately how long the camera movement takes (unfortunately not precise)")] private float travelTime = 1f;
    [SerializeField, Tooltip("Use current position as start")] private bool _isAtStartPos = false;
    [SerializeField, Tooltip("Use current rotation as start")] private bool _isAtStartRot = false;

    [Header("Start and End Positions")]
    [SerializeField] private Vector3 _startPosition = new Vector3(0f, 0f, 0f);
    [SerializeField] private Vector3 _endPosition = new Vector3(0f, 0f, 0f);

    [Header("Start and End Rotations")]
    [SerializeField, Tooltip("Whether the camera should rotate at all")] private bool _isRotate = true;
    [SerializeField] private Vector3 _startRotation = new Vector3(0f, 0, 0); 
    [SerializeField] private Vector3 _endRotation = new Vector3(0f, 90, 0);   

    [SerializeField, Tooltip("Distance from targetPos at which camera will stop smoothing and snap to goal")] private float _cameraSmoothingThreshold = 0.05f;

    [SerializeField, Tooltip("Delay before movement ACTUALLY starts")] private float _moveDelay = 0f;
    private float _delayTimer = 0f;
    bool isStarted = false;

    void Start()
    {
        if(!_isAtStartPos)
            this.transform.position = _startPosition;
        if (!_isAtStartRot)
            transform.eulerAngles = _startRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isStarted)
        {
            if (_delayTimer >= _moveDelay)
            {
                moveCamera();
                isStarted = true;
            }

            _delayTimer += Time.deltaTime;
        }
        
    }

    public void moveCamera()
    {
        StopAllCoroutines();
        if(_isRotate)
            StartCoroutine(DoCamPosition(_endPosition, travelTime, _endRotation));
        else
            StartCoroutine(DoCamPosition(_endPosition, travelTime, this.transform.rotation.eulerAngles));
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
}
