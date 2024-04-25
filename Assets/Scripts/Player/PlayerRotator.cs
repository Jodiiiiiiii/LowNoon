using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotator : MonoBehaviour
{
    private PlayerController _playerController;
    [SerializeField, Tooltip("duration of player gradually turning")] private float _turnTime = 3f;
    [SerializeField, Tooltip("Rotation the player is turning towards")] private Vector3 _targetRotation;

    private void Start()
    {
        _playerController = GetComponent<PlayerController>();
    }
    public void RotateFromMenuToGame()
    {
        StartCoroutine(DoGradualRotation());
    }

    IEnumerator DoGradualRotation()
    {
        float xAngle;
        float yAngle;
        float zAngle;

        float xVelocity = 0f;
        float yVelocity = 0f;
        float zVelocity = 0f;

        while (_playerController.enabled == false)
        {
            xAngle = Mathf.SmoothDampAngle(transform.eulerAngles.x, _targetRotation.x, ref xVelocity, _turnTime);
            yAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation.y, ref yVelocity, _turnTime);
            zAngle = Mathf.SmoothDampAngle(transform.eulerAngles.z, _targetRotation.z, ref zVelocity, _turnTime);

            transform.eulerAngles = new Vector3(xAngle, yAngle, zAngle);    // Change rotation
            yield return null;
        }
        yield return null;
    }
}
