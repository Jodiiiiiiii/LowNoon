using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Player objects - used for camera tracking and inputs
    [SerializeField] private PlayerController _player;
    private Transform _followTransform;

    [Header("Rotation Parameters / Constraints")]
    [SerializeField, Tooltip("look sensitivity")] private float _rotationSpeed = 1f;
    [SerializeField, Tooltip("camera rotation 'snappiness'")] private float _rotationSharpness = 1000f;
    [SerializeField, Tooltip("lowest possible look angle")] private float _minVertAngle = -85f;
    [SerializeField, Tooltip("highest possible look angle")] private float _maxVertAngle = 85f;

    [Header("Position Parameters / Constraints")]
    [SerializeField, Tooltip("camera panning 'snappiness'")] private float _followingSharpness = 1000f;
    [SerializeField, Tooltip("maximum distance from followTransform")] private float _maxDistance = 3f;
    [SerializeField, Tooltip("framing offset from followed target")] private Vector2 _framingOffset = Vector2.zero;

    // rotation calculation variables
    private Vector3 _targetPlanarDir = Vector3.forward;
    private float _targetVertAngle = 0f;
    // position calculation variables
    private Vector3 _currentFollowPosition; // smoothed panning origin

    // Start is called before the first frame update
    void Start()
    {
        _followTransform = _player.gameObject.transform;
        _currentFollowPosition = _followTransform.position;
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

        // Handle Obstructions

        // calculate orbit position based on smoothed rotation, smoothed follow position, and distance
        Vector3 targetPosition = _currentFollowPosition - (targetRotation * Vector3.forward * _maxDistance);

        // framing offset - shifts in screen view relative to final position on target
        targetPosition += transform.right * _framingOffset.x;
        targetPosition += transform.up * _framingOffset.y;

        // Apply calculated position
        transform.position = targetPosition;
        #endregion
    }
}
