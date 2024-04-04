using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : MonoBehaviour
{
    [Header("Reference Objects")]
    [Tooltip("Stores the ranged movement script on the Enemy")] public RangedMovement RangedMovement;
    [Tooltip("Stores bullet predab")] public GameObject BulletObject;
    private BulletStats _bulletStats; // retrieved from BulletObject above

    [Header("Bullet Stats")]
    [Tooltip("How far away from the player left and right the bullet will hit")] public float BulletSpread;

    [Header("Bullet Timer")]
    [Tooltip("How much time should pass between each shot")] public float ShotTimer;
    [Tooltip("Minimum amount of time to change the shot timer by")] public float MinShotVar;
    [Tooltip("Maximum amount of time to change the shot timer by")] public float MaxShotVar;
    
    private GameObject _player;
    private float _cooldownTimer;
    private float _shotTimerRandom;

    public bool IsAttacking;    // Used for animation
    
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        _cooldownTimer = ShotTimer + TimerRandom();
        _shotTimerRandom = ShotTimer + TimerRandom();

        _bulletStats = BulletObject.GetComponent<BulletStats>();
    }

    // Update is called once per frame
    void Update()
    {
        bool isStill = RangedMovement.StayStill;

        if (isStill && _cooldownTimer > _shotTimerRandom)
        {
            // Tell the animator we're attacking
            IsAttacking = true;
            // reset cooldown
            _shotTimerRandom = ShotTimer + TimerRandom();
            _cooldownTimer = 0;

            // create bullet at enemy, facing player
            GameObject bullet = Instantiate(BulletObject);
            bullet.transform.position = transform.position; // TODO: parameters/equation to make bullet align with enemy gun
            bullet.transform.LookAt(_player.transform.position);

            // randomize y (planar) rotation within BulletSpread range
            Vector3 bulletRotation = bullet.transform.rotation.eulerAngles;
            bulletRotation.y += Random.Range(-BulletSpread, BulletSpread);
            bullet.transform.rotation = Quaternion.Euler(bulletRotation);

            // apply bullet force
            bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * _bulletStats.InitialForce, ForceMode.Impulse);
        }
        else
        {
            IsAttacking = false;
        }

        _cooldownTimer += Time.deltaTime;
    }

    float TimerRandom()
    {
        return Random.Range(MinShotVar, MaxShotVar);
    }
}
