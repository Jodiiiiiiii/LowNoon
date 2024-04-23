using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Reference to attack object under the enemy")]public Collider AttackCollider;

    [Header("Timers")]
    [Tooltip("How long the attack animation takes; duration that damage hitbox is out")]public float AttackTime;
    [Tooltip("How long between each attack")]public float AttackCooldown;
    [Tooltip("Distance from the player when the enemy will initiate an attack")] public float AttackRange;

    private Collider _player;
    private DamageReceiver health;
    private float _attackTimer;
    private bool _duringAttack;
    public bool DuringAttack => _duringAttack;
    
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<Collider>();
        health = GetComponent<DamageReceiver>();
        _duringAttack = false;
        _attackTimer = 0;
        AttackCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        _attackTimer += Time.deltaTime;

        if (_duringAttack && _attackTimer > AttackTime) // if attack is completed
        {
            // end attack state
            _duringAttack = false;
            AttackCollider.enabled = false;
            _attackTimer = 0;
        }

        if (Vector3.Distance(AttackCollider.transform.position, _player.ClosestPoint(AttackCollider.transform.position)) <= AttackRange) // if close to player
        {
            if (!_duringAttack && _attackTimer > AttackCooldown) // if attack is off cooldown and ready
            {
                // initiate attack
                _duringAttack = true;
                AttackCollider.enabled = true;
                _attackTimer = 0;
            }
        }

        // disable attack collider if dead
        if (health.HealthLevel <= 0)
            AttackCollider.enabled = false;
    }
}
