using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MeleeMovement : MonoBehaviour
{
    private GameObject _Player;
    private Vector3 _PlayerPosition;

    public float MoveSpeed = 5f;

    private Rigidbody _rigidBody;


    // Start is called before the first frame update
    void Start()
    {
        _Player = GameObject.FindWithTag("Player");
        _rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    // Reference: https://www.youtube.com/watch?v=ZExSz7x69j8
    void Update()
    {
        _PlayerPosition = _Player.transform.position;

        // Set enemy rotation to face player
        transform.LookAt(_PlayerPosition);

        // Move enemy towards player
        // If enemy is within 1 unit of player, stop moving
        if (Vector3.Distance(_PlayerPosition, transform.position) > 2f)
        {
            Vector3 direction = (_PlayerPosition - transform.position).normalized * MoveSpeed;
            direction.y = _rigidBody.velocity.y;
            _rigidBody.velocity = direction;
        }
        else
        {
            _rigidBody.velocity = new Vector3(0, 0, 0);
        }
    }
}
