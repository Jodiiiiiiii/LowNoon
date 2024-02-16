using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Max Health")]
    [SerializeField, Tooltip("Base health stat")] private int _baseHealth = 20;
    [SerializeField, Tooltip("Max health gained per upgrade")] private int _healthPerUpgrade = 5;

    [Header("Armor")]
    [SerializeField, Tooltip("Base armor stat")] private int _baseArmor = 0;
    [SerializeField, Tooltip("Armor gained per upgrade")] private int _armorPerUpgrade = 1;

    [Header("Bullet Damage")]
    [SerializeField, Tooltip("Base bullet damage stat")] private float _baseBulletDamage = 1f;
    [SerializeField, Tooltip("Bullet damage gained per upgrade")] private float _bulletDamagePerUpgrade = 0.5f;

    [Header("Fire Rate")]
    [SerializeField, Tooltip("Base bullet cooldown stat")] private float _baseBulletCooldown = 0.8f;
    [SerializeField, Tooltip("Multiplied factor per bullet cooldown upgrade")] private float _bulletCooldownUpgradeFactor = 0.8f;

    [Header("Move Speed")]
    [SerializeField, Tooltip("Base move speed stat - influences move force, max speed, and turning speeds multiplicatively")] private float _baseMoveSpeed = 1f;
    [SerializeField, Tooltip("Move speed per upgrade")] private float _moveSpeedPerUpgrade = 0.1f;
    [SerializeField, Tooltip("Base dash damage stat")] private float _baseDashDamage = 2f;
    [SerializeField, Tooltip("Dash damage per upgrade")] private float _dashDamagePerUpgrade = 1f;

    [Header("Light")]
    [SerializeField, Tooltip("Base light FOV stat")] private float _baseLightFOV = 45f;
    [SerializeField, Tooltip("Interpolation ratio for increasing FOV from current to max (90 degrees)")] private float _lightFOVLerpRatio = 0.3f;
    [SerializeField, Tooltip("Base light intensity")] private float _baseLightIntensity = 0.5f;
    [SerializeField, Tooltip("Interpolation ratio for increasing intensity from current to max (= 1)")] private float _lightIntensityLerpRatio = 0.3f;

    /// <summary>
    /// generates set of player data with base stats
    /// </summary>
    public GameManager.Stats GetBasePlayerData()
    {
        GameManager.Stats baseData = new();

        // health
        baseData.MaxHealth = _baseHealth;
        baseData.CurrHealth = _baseHealth;
        // armor
        baseData.Armor = _baseArmor;
        // bullet damage
        baseData.BulletDamage = _baseBulletDamage;
        // fire rate
        baseData.BulletCooldown = _baseBulletCooldown;
        // move speed
        baseData.MoveSpeed = _baseMoveSpeed;
        baseData.DashDamage = _baseDashDamage;
        // light
        baseData.LightFOV = _baseLightFOV;
        baseData.LightIntensity = _baseLightIntensity;

        return baseData;
    }

    /// <summary>
    /// generates new set of player data with a certain upgrade applied
    /// </summary>
    public GameManager.Stats ApplyUpgrade(UpgradeController.UpgradeType upgradeType)
    {
        GameManager.Stats playerData = GameManager.Instance.PlayerData;

        switch(upgradeType)
        {
            case UpgradeController.UpgradeType.Health:
                playerData.MaxHealth += _healthPerUpgrade;
                playerData.CurrHealth += _healthPerUpgrade;
                break;
            case UpgradeController.UpgradeType.Armor:
                playerData.Armor += _armorPerUpgrade;
                break;
            case UpgradeController.UpgradeType.Damage:
                playerData.BulletDamage += _bulletDamagePerUpgrade;
                break;
            case UpgradeController.UpgradeType.FireSpeed:
                playerData.BulletCooldown *= _bulletCooldownUpgradeFactor;
                break;
            case UpgradeController.UpgradeType.MoveSpeed:
                playerData.MoveSpeed += _moveSpeedPerUpgrade;
                playerData.DashDamage += _dashDamagePerUpgrade;
                break;
            case UpgradeController.UpgradeType.Light:
                playerData.LightFOV = Mathf.Lerp(playerData.LightFOV, 90, _lightFOVLerpRatio); // 90 = max FOV
                playerData.LightIntensity = Mathf.Lerp(playerData.LightIntensity, 1, _lightIntensityLerpRatio); // 1 = max intensity
                break;
        }

        return playerData;
    }
}
