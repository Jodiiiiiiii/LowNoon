using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeMovement : MonoBehaviour
{
    public GameObject Player;
    public Vector3 PlayerPosition;

    public GameObject Ground;
    public Vector3 GroundPosition;

    public float MoveSpeed = 5f;


    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        Ground = GameObject.FindWithTag("Ground");
    }

    // Update is called once per frame
    void Update()
    {
        PlayerPosition = Player.transform.position;

        // Set enemy rotation to face player
        transform.LookAt(PlayerPosition);

        // Move enemy towards player
        // If enemy is within 1 unit of player, stop moving
        if (Vector3.Distance(PlayerPosition, transform.position) > 1)
        {
            transform.position += transform.forward * MoveSpeed * Time.deltaTime;
        }
    }
}
