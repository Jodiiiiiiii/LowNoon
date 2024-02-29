using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    // Player animations essentials
    private PlayerController _playerController;
    [SerializeField] private Animator _animator;

    // Accessory animation essentials
    [SerializeField, Tooltip("The model for the player's hat")] private GameObject _hat;
    [SerializeField, Tooltip("The model for the player's lamp")] private GameObject _lamp;
    private Animator _hatAnimator;  // The hat and lamp animators are used p. much exclusively for the death animation
    private Animator _lampAnimator;

    // Gun GameObject and animator no longer required now that gun is parented to player + doesn't need the firing animation


    private void OnEnable()
    {
        PlayerShooting.onBulletFire += fireGun;
    }

    private void OnDisable()
    {
        PlayerShooting.onBulletFire -= fireGun;
    }

    void Start()
    {
        _playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        _animator.SetFloat("moveSpdMult", GameManager.Instance.PlayerData.MoveSpeed); // Move speed multiplier so animation stays tuned to actual speed

        // Player animator is primarily controlled by whether the player is (a) dashing and (b) moving
        if(_playerController.State == CharacterState.STATIONARY)
        {
            if (_playerController.State == CharacterState.DASH)
            {
                StopAllCoroutines();
                _animator.SetBool("isDashing", true);             
            }
            else
            {
                _animator.SetBool("isDashing", false);
                _animator.SetBool("isMoving", false);
            }
            
        }
        else
        {
            if (_playerController.State == CharacterState.DASH)
            {
                StopAllCoroutines();
                _animator.SetBool("isDashing", true);
                
            }
            else
            {
                StopAllCoroutines();
                _animator.SetBool("isDashing", false);
                _animator.SetBool("isMoving", true);
            }
            
        }
   
    }

    private void fireGun() // Method that does prep for the coroutine and calls it (can't call a coroutine with a delegate)
    {
        StopAllCoroutines();
        _animator.Play("Idle", 0, 0);
        StartCoroutine(DoFireGun());
    }

    private IEnumerator DoFireGun() // Unique sequence for firing the gun
    {
        _animator.Play("Fire", 0, 0);
        yield return new WaitForSeconds(.5f);
        _animator.Play("Idle", 0, 0);

    }
}
