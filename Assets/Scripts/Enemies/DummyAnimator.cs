using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyAnimator : EnemyAnimator
{
    // Start is called before the first frame update
    [SerializeField] protected float _reviveAnimDuration;
    private float _maxHealth;
    void Start()
    {
        base.Start();
        _maxHealth = _damageReceiver.HealthLevel; // Store max HP to use to revive later
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    protected override IEnumerator DoDeathAnim() // The targets spring back up after a little if you want 'em to
    {
        _damageReceiver.IsImmune = true;
        _animator.SetBool("isDead", true);
        yield return new WaitForSeconds(_deathAnimDuration);
        _animator.SetBool("isDead", false);
        _damageReceiver.HealthLevel = _maxHealth;
        _damageReceiver.IsImmune = false;
        yield return new WaitForSeconds(_reviveAnimDuration);
        

    }
}
