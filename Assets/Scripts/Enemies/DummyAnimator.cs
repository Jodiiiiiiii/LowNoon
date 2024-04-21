using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyAnimator : EnemyAnimator
{
    // Start is called before the first frame update
    [SerializeField] protected float _reviveAnimDuration;
    private float _maxHealth;
    private BoxCollider _collider;
    
    
    new void Start()
    {
        base.Start();
        _maxHealth = _damageReceiver.HealthLevel; // Store max HP to use to revive later
        _collider = GetComponent<BoxCollider>();
        
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }

    protected override IEnumerator DoDeathAnim() // The targets spring back up after a little if you want 'em to
    {
        _collider.enabled = false;
        _damageReceiver.IsImmune = true;
        _animator.SetBool("isDead", true);
        _audioSource.PlayOneShot(_clips[1], 1.0f);
        yield return new WaitForSeconds(_deathAnimDuration);
        
        _animator.SetBool("isDead", false);
        _damageReceiver.HealthLevel = _maxHealth;
        _damageReceiver.IsImmune = false;
        _collider.enabled = true;
        yield return new WaitForSeconds(_reviveAnimDuration);
        

    }
}
