using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    // Player animations essentials
    private PlayerController _playerController;
    [SerializeField] private Animator _animator;

    // Accessory animation essentials
    [SerializeField, Tooltip("The model for the player's gun")] private GameObject _gun;
    [SerializeField, Tooltip("The model for the player's hat")] private GameObject _hat;
    [SerializeField, Tooltip("The model for the player's lamp")] private GameObject _lamp;
    [SerializeField] private Animator _gunAnimator;  // The gun animator is used for having the gun be either idle or firing
    private Animator _hatAnimator;  // The hat and lamp animators are used p. much exclusively for the death animation
    private Animator _lampAnimator;

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

        //_gunAnimator = _gun.GetComponent<Animator>();
        _gun.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(_playerController.State == CharacterState.STATIONARY)
        {
            _animator.SetBool("isMoving", false);
            _gun.SetActive(true);
        }
        else if(_playerController.State == CharacterState.MOVING)
        {
            _animator.SetBool("isMoving", true);
            _gun.SetActive(false);
        }
    }

    private void fireGun()
    {
        StartCoroutine(DoFireGun());
    }

    private IEnumerator DoFireGun()
    {
        _animator.Play("Fire", 0, 0);
        _gunAnimator.SetBool("fire", true);
        yield return new WaitForSeconds(.75f);
        
        //yield return new WaitForSeconds(.5f);
        _animator.Play("Idle", 0, 0);
        _gunAnimator.SetBool("fire", false);

    }
}
