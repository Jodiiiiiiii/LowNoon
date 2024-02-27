using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public bool IsInvulnerable { get; private set; } = false;
    [Tooltip("Time after taking damage during which the player cannot take more damage")] public float InvulnerabilityDuration = 5.0f;
    float _timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {   }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer > InvulnerabilityDuration) // check for invulnerability time expiration
        {
            IsInvulnerable = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject DamagerObject = collision.gameObject;
        
        if (DamagerObject.CompareTag("EnemyBullet"))
        {
            BulletStats bulletStats = DamagerObject.GetComponent<BulletStats>();
            
            // destroy bullet always on impact with player
            Destroy(DamagerObject);

            if (!IsInvulnerable)
            {
                // consume 1 armor if any present, otherwise simply apply damage to health
                if (GameManager.Instance.PlayerData.Armor > 0) GameManager.Instance.PlayerData.Armor -= 1;
                else GameManager.Instance.PlayerData.CurrHealth -= (int) bulletStats.DamageLevel;

                IsInvulnerable = true;
                _timer = 0.0f; // resets timer
            }
            
            // check if player reaches 0 health
            if (GameManager.Instance.PlayerData.CurrHealth <= 0)
            {
                // player dies
                // TODO: integrate animations and death state transition to restart scene
                gameObject.SetActive(false); // temporary death behavior for testing purposes
            }
        }
    }


}
