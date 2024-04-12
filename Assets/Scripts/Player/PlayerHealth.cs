using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public bool IsInvulnerable { get; private set; } = false;
    [Tooltip("Time after taking damage during which the player cannot take more damage")] public float InvulnerabilityDuration = 5.0f;
    float _timer = 0.0f;

    [SerializeField, Tooltip("damage dealt to the player by enemy melee attacks (ants)")] private int _meleeEnemyDamage = 1;

    [System.NonSerialized] public int ExplosionDmg;
    private PlayerController _player;
    

    // Start is called before the first frame update
    void Start()
    {
        _player = gameObject.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer > InvulnerabilityDuration) // check for invulnerability time expiration
        {
            IsInvulnerable = false;
        }
        if(ExplosionDmg > 0){
            handleDamage(ExplosionDmg);
            ExplosionDmg = 0;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject DamagerObject = collision.gameObject;

        if (DamagerObject.CompareTag("EnemyBullet"))
        {
            BulletStats bulletStats = DamagerObject.GetComponent<BulletStats>();
            if (bulletStats == null)
            {
                bulletStats = DamagerObject.GetComponentInChildren<BulletStats>();
            }

            handleDamage((int)bulletStats.DamageLevel);

            Destroy(DamagerObject); // supposed to slightly mitigate effecct of bullet pushing player when it hits briefly
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MeleeAttack") && _player.State != CharacterState.DASH)
        {
            handleDamage(_meleeEnemyDamage);
        }

        if (other.CompareTag("HealthPickup"))
        {
            if (GameManager.Instance.PlayerData.CurrHealth < GameManager.Instance.PlayerData.MaxHealth)
            {
                GameManager.Instance.PlayerData.CurrHealth++;
            }
            Destroy(other.gameObject);
        }
    }

    private void handleDamage(int dmgAmount)
    {
        if (!IsInvulnerable)
        {
            // consume 1 armor if any present, otherwise simply apply damage to health
            if (GameManager.Instance.PlayerData.Armor > 0) GameManager.Instance.PlayerData.Armor -= 1;
            else GameManager.Instance.PlayerData.CurrHealth -= dmgAmount;

            IsInvulnerable = true;
            _timer = 0.0f; // resets timer
        }

        // check if player reaches 0 health
        if (GameManager.Instance.PlayerData.CurrHealth <= 0)
        {
            // player dies
            // TODO: integrate animations and death state transition to restart scene
            ViewManager.Show<GameOverView>(false);
        }
    }
}
