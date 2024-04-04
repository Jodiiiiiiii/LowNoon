using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntAnimator : EnemyAnimator
{
    [SerializeField] private GameObject _vanishEffect;

    private MeleeMovement _meleeMovement;
    new void Start()
    {
        base.Start();
        _meleeMovement = GetComponent<MeleeMovement>();
    }

    // Update is called once per frame
    new void Update()
    {
        _animator.SetBool("isMoving", !_meleeMovement.IsIdle);
        _animator.SetBool("isAttacking", _meleeMovement.IsAtPlayer);

        if (prevHP > _damageReceiver.HealthLevel)    // If the enemy has been damaged
        {
            if (_damageReceiver.HealthLevel <= 0 && !_animator.GetBool("isDead")) // If the enemy is dead
            {
                StartCoroutine(DoDeathAnim());
            }
            else // If only hurt
            {
                // Ant has no hurt anim, 2 small, but there WILL be a sound
            }
        }
        prevHP = _damageReceiver.HealthLevel;
    }
    protected override IEnumerator DoDeathAnim()
    {
        _animator.SetBool("isDead", true);
        yield return new WaitForSeconds(_deathAnimDuration);
        Instantiate(_vanishEffect, this.transform.position, _vanishEffect.transform.rotation);
        Destroy(this.gameObject);
    }
}
