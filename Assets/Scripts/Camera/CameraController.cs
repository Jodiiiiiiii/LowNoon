using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Player")]
    [SerializeField, Tooltip("used for camera tracking and inputs")] private PlayerController _player;
    [SerializeField, Tooltip("the geometry around which the camera tracks")] private Transform _followTransform;

    [Header("Rotation Parameters / Constraints")]
    [SerializeField, Tooltip("look sensitivity")] private float _rotationSpeed = 1f;
    [SerializeField, Tooltip("camera rotation 'snappiness'")] private float _rotationSharpness = 1000f;
    [SerializeField, Tooltip("lowest possible look angle")] private float _minVertAngle = -85f;
    [SerializeField, Tooltip("highest possible look angle")] private float _maxVertAngle = 85f;

    [Header("Position Parameters / Constraints")]
    [SerializeField, Tooltip("camera panning 'snappiness'")] private float _followingSharpness = 1000f;
    [SerializeField, Tooltip("maximum distance from followTransform")] private float _maxDistance = 3f;
    [SerializeField, Tooltip("framing offset from followed target")] private Vector2 _framingOffset = Vector2.zero;

    [Header("Obstructions")]
    [SerializeField, Tooltip("camera zoom 'snappiness'")] private float _zoomSharpness = 1000f;
    [SerializeField, Tooltip("radius of sphere cast for obstruction detection")] private float _obstructionCheckRadius = 2f;
    [SerializeField, Tooltip("maximum number of obstructing objects detected in a single sphere cast")] private int _maxObstructions = 32;
    [SerializeField, Tooltip("layers considered for obstruction checks")] private LayerMask _obstructionLayers;

    // rotation calculation variables
    private Vector3 _targetPlanarDir = Vector3.forward;
    private float _targetVertAngle = 0f;
    // position calculation variables
    private Vector3 _currentFollowPosition; // smoothed panning origin
    private float _currentDistance;

    // Start is called before the first frame update
    void Start()
    {

        _currentFollowPosition = _followTransform.position;
        _currentDistance = _maxDistance;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        #region rotation
        // planar/horizontal rotation input
        Quaternion planarChange = Quaternion.Euler(_followTransform.up * _player.PlayerInput.LookAxisRight * _rotationSpeed); // horizontal change
        _targetPlanarDir = planarChange * _targetPlanarDir; // rotate target by change
        Quaternion planarRot = Quaternion.LookRotation(_targetPlanarDir, _followTransform.up); // quaternion representation of _targetPlanarDir

        // process vertical rotation input
        _targetVertAngle -= _player.PlayerInput.LookAxisUp * _rotationSpeed; // vertical change
        _targetVertAngle = Mathf.Clamp(_targetVertAngle, _minVertAngle, _maxVertAngle);
        Quaternion verticalRot = Quaternion.Euler(_targetVertAngle, 0, 0); // quaternion representation of _targetVertAngle

        // smoothly interpolate target rotation
        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, planarRot * verticalRot, 1f - Mathf.Exp(-_rotationSharpness * Time.deltaTime));

        // apply calculated rotation
        transform.rotation = targetRotation;
        #endregion

        #region position
        // smoothly lerp central follow position to followTransform
        _currentFollowPosition = Vector3.Lerp(_currentFollowPosition, _followTransform.position, 1f - Mathf.Exp(-_followingSharpness * Time.deltaTime));

        // framing offset - shifts in screen view before obstruction checking
        _currentFollowPosition += transform.right * _framingOffset.x;
        _currentFollowPosition += transform.up * _framingOffset.y;

        // Handle Obstructions
        RaycastHit closestHit = new();
        closestHit.distance = Mathf.Infinity; // collision distance (infinity by default = no collision)
        RaycastHit[] obstructions = new RaycastHit[_maxObstructions];
        int obstructionCount = Physics.SphereCastNonAlloc(_currentFollowPosition, _obstructionCheckRadius, -transform.forward, obstructions, _maxDistance, _obstructionLayers, QueryTriggerInteraction.Ignore);
        // find closest obstruction
        for(int i = 0; i < obstructionCount; i++)
        {
            if (obstructions[i].distance < closestHit.distance && obstructions[i].distance > 0) closestHit = obstructions[i];
        }

        // smoothly lerp to desired zoom distance (either max or nearest obstruction)
        if (closestHit.distance < Mathf.Infinity)
            _currentDistance = Mathf.Lerp(_currentDistance, closestHit.distance, 1 - Mathf.Exp(-_zoomSharpness * Time.deltaTime));
        else
            _currentDistance = Mathf.Lerp(_currentDistance, _maxDistance, 1 - Mathf.Exp(-_zoomSharpness * Time.deltaTime));

        // calculate orbit position based on smoothed rotation, smoothed follow position, and smoothed distance
        Vector3 targetPosition = _currentFollowPosition - (targetRotation * Vector3.forward * _currentDistance);

        // Apply calculated position
        transform.position = targetPosition;
        #endregion
    }
}