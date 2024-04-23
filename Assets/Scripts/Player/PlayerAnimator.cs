using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    // Player animations essentials
    private PlayerController _playerController;
    private PlayerShooting _shooting;
    private AudioSource _audioSource;
    [SerializeField] private Animator _animator;
    [Tooltip("Audio clip order list: 0 = standard gunshot; 1 = Standard reload; 2 = six shot reload; 3 = burrowing sound")]
    [SerializeField] private List<AudioClip> _clips = new List<AudioClip>();

    // TODO: figure out actual value to make click sound time out best; currently 0 because of fire animation interruption issue (there is a bug card on Trello)
    [SerializeField, Tooltip("time before reload is ready that the audio clip starts playing")] private float _reloadClickOffset = 0.0f;
    [SerializeField] private float _gunFireVolume = 0.4f;

    // Accessory animation essentials
    [SerializeField, Tooltip("The model for the player's hat")] private GameObject _hat;
    [SerializeField, Tooltip("The model for the player's lamp")] private GameObject _lamp;

    private Animator _hatAnimator;  // The hat and lamp animators are used p. much exclusively for the death animation
    private Animator _lampAnimator;
    private int _sixShotCount;
    private bool _isActiveCoroutine;    // Used to turn off standard animation logic when a unique animation is playing

    // Unique animation durations
    public float BurrowDownDuration = 1.5167f;
    public float RoomEnterDuration = 1.2083f;

    private void OnEnable()
    {
        PlayerShooting.onBulletFire += fireGun;
        SceneTransitionObject.onSceneTransition += burrowDown;
        GameManager.onSceneBegin += roomEnter;
        GameManager.onHubRevive += hubEnter;
    }

    private void OnDisable()
    {
        PlayerShooting.onBulletFire -= fireGun;
        SceneTransitionObject.onSceneTransition -= burrowDown;
        GameManager.onSceneBegin -= roomEnter;
        GameManager.onHubRevive -= hubEnter;
    }

    void Start()
    {
        _playerController = GetComponent<PlayerController>();
        _shooting = GetComponent <PlayerShooting>();
        _audioSource = GetComponent<AudioSource>();
        //_hatAnimator = _hat.GetComponent<Animator>(); // TODO: add this back when it is actually there
        //_lampAnimator = _lamp.GetComponent<Animator>(); // TODO: add this back when it is actually there
        _sixShotCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isActiveCoroutine)
        {
            _animator.SetFloat("moveSpdMult", GameManager.Instance.PlayerData.MoveSpeed); // Move speed multiplier so animation stays tuned to actual speed

            if(GameManager.Instance.PlayerData.CurrHealth <= 0)
            {
                defeat();
            }

            if (_playerController.enabled == false)  // If the player lacks control, default the animation to the idle
            {
                _animator.SetBool("isDashing", false);
                _animator.SetBool("isMoving", false);
            }
            else // player has control
            {
                // Player animator is primarily controlled by whether the player is (a) dashing and (b) moving
                if (_playerController.State == CharacterState.STATIONARY)
                {
                    _animator.SetBool("isMoving", false);
                    _animator.SetBool("isDashing", false);
                }
                else if (_playerController.State == CharacterState.DASH)
                {
                    StopAllCoroutines();
                    _animator.SetBool("isDashing", true);
                    

                }
                else // moving (not STATIONARY or DASH)
                {
                    StopAllCoroutines();
                    _animator.SetBool("isDashing", false);
                    _animator.SetBool("isMoving", true);
                }
            }
        }
        
    }

    private void fireGun() // Methods that do prep for the coroutines and call them (can't call a coroutine with a delegate)
    {
        StopAllCoroutines();
        _animator.Play("Idle", 0, 0);
        StartCoroutine(DoFireGun());
    }

    private void burrowDown()
    {
        StopAllCoroutines();
        StartCoroutine(DoBurrowDown());
    }

    private void roomEnter()
    {
        StopAllCoroutines();
        //_animator.Play("Idle", 0, 0);
        StartCoroutine(DoRoomEnter(0, false));
    }

    private void hubEnter()
    {
        StopAllCoroutines();
        //_animator.Play("Idle", 0, 0);
        StartCoroutine(DoRoomEnter(4, true));
    }

    private void defeat()
    {
        StopAllCoroutines();
        StartCoroutine(DoDefeat());
    }

    #region COROUTINES
    private IEnumerator DoFireGun() // Unique sequence for firing the gun
    {
        _audioSource.PlayOneShot(_clips[0], _gunFireVolume * GameManager.Instance.GetPlayerVolume());
        _animator.Play("Fire", 0, 0);
        _sixShotCount++;
        
        yield return new WaitForSeconds(GameManager.Instance.PlayerData.BulletCooldown - _reloadClickOffset); // make sure click finishes right when you can fire again
        if(_sixShotCount >= 6) // >= so that it doesn't skip 6 when it skips audio due to moving (why does it do this exactly?)
        {
            _audioSource.PlayOneShot(_clips[2], _gunFireVolume * GameManager.Instance.GetPlayerVolume());
            _sixShotCount = 0;
        }
        else
        {
            _audioSource.PlayOneShot(_clips[1], _gunFireVolume * GameManager.Instance.GetPlayerVolume());
        }
            
        _animator.Play("Idle", 0, 0);
    }

    private IEnumerator DoBurrowDown() // Unique sequence for leaving a room
    {
        _isActiveCoroutine = true;
        _animator.SetBool("isBurrowDown", true);
        _audioSource.PlayOneShot(_clips[3],GameManager.Instance.GetPlayerVolume());
        yield return new WaitForSeconds(BurrowDownDuration); // Unity, why is there not a way to tell when an animation is done, it would save me so much heartache
    }

    private IEnumerator DoRoomEnter(float waitTime, bool revive) // Unique sequence for entering a room
    {
        _playerController = GetComponent<PlayerController>();
        _shooting = GetComponent<PlayerShooting>();
        _isActiveCoroutine = true;

        _playerController.enabled = false;
        _shooting.enabled = false;
        if (revive)
        {
            GameObject.Find("Title Music Audio").SetActive(false);
        }
            

       _animator.Play("RoomEnter", 0, 0);
        _animator.SetFloat("roomEnterPause", 0);
        yield return new WaitForSeconds(waitTime);

        _animator.SetFloat("roomEnterPause", 1);
        if (revive)
        {
            GameObject.Find("Player Camera").GetComponent<ManualCameraController>().moveToReviveStart();
            GameObject.Find("Ambient Audio").GetComponent<MusicController>().FadeIn();
        }
            
        yield return new WaitForSeconds(RoomEnterDuration);

        GameObject.Find("Player Camera").GetComponent<CameraController>().enabled = true;
        _playerController.enabled = true;
        _shooting.enabled = true;

        _playerController.Reenable();   // Give the player back control

        _animator.Play("Idle", 0, 0);
        _isActiveCoroutine = false; 
    }

    private IEnumerator DoDefeat()
    {
        _isActiveCoroutine = true;
        _playerController.enabled = false;
        _shooting.enabled = false;
        GameObject.Find("Player Camera").GetComponent<CameraController>().enabled = false;

        _animator.Play("Death", 0, 0);
        //_hatAnimator.Play("Fall", 0, 0); // TODO: add this back when it is actually there
        //_lampAnimator.Play("Fall", 0, 0); // TODO: add this back when it is actually there
        yield return null;
    }
    #endregion
}
