using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MeleeMovement : MonoBehaviour
{
    private GameObject _player;
    private Vector3 _playerPosition;

    public float MoveSpeed = 5f;

    private Rigidbody _rigidBody;


    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        _rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    // Reference: https://www.youtube.com/watch?v=ZExSz7x69j8
    // https://discussions.unity.com/t/fastest-way-to-set-z-axis-rotation-to-0-c/220293
    void Update()
    {
        _playerPosition = _player.transform.position;

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
}
