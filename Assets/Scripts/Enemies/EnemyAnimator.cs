using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    // Enemy animator variables:
        // isHurting
        // isDead

    protected DamageReceiver _damageReceiver;
    [SerializeField] protected Animator _animator;
    protected float prevHP;

    [Header("Bespoke Animation Durations")]
    [SerializeField] protected float _hurtAnimDuration;
    [SerializeField] protected float _deathAnimDuration;
    [SerializeField] protected float _attackAnimDuration;
    protected AudioSource _audioSource;
    [Tooltip("Audio clip order list: 0 = Damage Sound; 1 = Falling sound")]
    [SerializeField] protected List<AudioClip> _clips = new List<AudioClip>();
    // Start is called before the first frame update
    protected void Start()
    {
        _damageReceiver = GetComponent<DamageReceiver>();
        if (GetComponent<Animator>() != null)
        {
            _animator = GetComponent<Animator>();
        }
        prevHP = _damageReceiver.HealthLevel;
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    protected void Update()
    {
        _animator.SetFloat("HP", _damageReceiver.HealthLevel);
        if(prevHP > _damageReceiver.HealthLevel)    // If the enemy has been damaged
        {
            if(_damageReceiver.HealthLevel <= 0) // If the enemy is dead
            {
                StartCoroutine(DoDeathAnim());
            }
            else // If only hurt
            {
                StartCoroutine(DoHurtAnim());
            }
        }
        prevHP = _damageReceiver.HealthLevel;
    }

    // Idle and movement should be controlled entirely in Update() by whether or not the enemy is moving

    // Bespoke animations / things that exist outside of the animation loop rotation normally:
    protected virtual IEnumerator DoAttackAnim() { yield return null; }
    protected virtual IEnumerator DoHurtAnim() {

        _animator.SetBool("isHurting", true);
        _audioSource.PlayOneShot(_clips[0], 1.0f);
        yield return new WaitForSeconds(_hurtAnimDuration);
        _animator.SetBool("isHurting", false);
    }
    // Dying
    protected virtual IEnumerator DoDeathAnim() {
        _animator.SetBool("isDead", true);
        yield return new WaitForSeconds(_deathAnimDuration);
        // Insert particle FX for disappearing in here
        Destroy(this.gameObject);
    }
}
