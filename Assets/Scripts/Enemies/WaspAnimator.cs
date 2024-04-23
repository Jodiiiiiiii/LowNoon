using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaspAnimator : EnemyAnimator
{
    [SerializeField] private GameObject _vanishEffect;

    private RangedAttack _rangedAttack;
    private RangedMovement _rangedMovement;

    private bool _isActiveCoroutine;
    private float _timer = 0.0f;
    
    new void Start()
    {
        base.Start();
        _rangedMovement = GetComponent<RangedMovement>();
        _rangedAttack = GetComponent<RangedAttack>();
        
    }

    // Update is called once per frame
    new void Update()
    {   _timer -= Time.deltaTime;
        if(_timer <= 0){
            _audioSource.PlayOneShot(_clips[2], 0.3f * GameManager.Instance.GetEnemyVolume());
            _timer = 5.5f;
        }
        if(_rangedMovement.EnemyMoveState == RangedMovement.RangeEnemyMoveState.ZIPPY) // Movement animations
        {
            if (_rangedMovement.Right)
            {
                _animator.SetBool("isMovingRight", true);
                _animator.SetBool("isMovingLeft", false);
            }
            else
            {
                _animator.SetBool("isMovingRight", false);
                _animator.SetBool("isMovingLeft", true);
            }
        }
        else
        {
            _animator.SetBool("isMovingRight", false);
            _animator.SetBool("isMovingLeft", false);
        }

        if (_rangedAttack.IsAttacking)
        {
            StartCoroutine(DoAttackAnim());
        }

        if (prevHP > _damageReceiver.HealthLevel)    // If the enemy has been damaged
        {
            if (_damageReceiver.HealthLevel <= 0 && !_animator.GetBool("isDead")) // If the enemy is dead
            {
                StartCoroutine(DoDeathAnim());
            }
            else // If only hurt
            {
                // Wasp has no hurt anim; use for sound
            }
        }
        prevHP = _damageReceiver.HealthLevel;
    }

    protected override IEnumerator DoAttackAnim() { 
        _isActiveCoroutine = true;
        _animator.SetBool("isAttacking", true);
        _animator.Play("Attack", 0, 0);
        _audioSource.PlayOneShot(_clips[3], 0.5f * GameManager.Instance.GetEnemyVolume());
        yield return new WaitForSeconds(_attackAnimDuration);
        _animator.SetBool("isAttacking", false);
        _isActiveCoroutine = false;
        yield return null; 
    }

    protected override IEnumerator DoDeathAnim()
    {
        _animator.SetBool("isDead", true);
        yield return new WaitForSeconds(_deathAnimDuration);
        Instantiate(_vanishEffect, this.transform.position, _vanishEffect.transform.rotation);
        Destroy(this.gameObject);
    }
}
