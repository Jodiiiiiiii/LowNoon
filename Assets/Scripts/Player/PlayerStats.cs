using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Max Health")]
    [SerializeField, Tooltip("Base health stat")] private int _baseHealth = 5;
    [SerializeField, Tooltip("Max health gained per upgrade")] private int _healthPerUpgrade = 1;

    [Header("Armor")]
    [SerializeField, Tooltip("Base armor stat")] private int _baseArmor = 0;
    [SerializeField, Tooltip("Armor gained per upgrade")] private int _armorPerUpgrade = 1;

    [Header("Bullet Damage")]
    [SerializeField, Tooltip("Base bullet damage stat")] private float _baseBulletDamage = 1f;
    [SerializeField, Tooltip("Bullet damage gained per upgrade")] private float _bulletDamagePerUpgrade = 0.5f;

    [Header("Fire Rate")]
    [SerializeField, Tooltip("Base bullet cooldown stat")] private float _baseBulletCooldown = 0.8f;
    [SerializeField, Tooltip("Multiplied factor per bullet cooldown upgrade")] private float _bulletCooldownUpgradeFactor = 0.8f;
    private int _baseFireRateUpgradeCount = 0;

    [Header("Move Speed")]
    [SerializeField, Tooltip("Base move speed stat - influences move force, max speed, and turning speeds multiplicatively")] private float _baseMoveSpeed = 1f;
    [SerializeField, Tooltip("Move speed per upgrade")] private float _moveSpeedPerUpgrade = 0.1f;

    [Header("Light")]
    [SerializeField, Tooltip("Base light FOV stat")] private float _baseLightFOV = 45f;
    [SerializeField, Tooltip("Max light FOV stat")] private float _maxLightFOV = 160f;
    [SerializeField, Tooltip("Interpolation ratio for increasing FOV from current to max")] private float _lightFOVLerpRatio = 0.3f;
    [SerializeField, Tooltip("Base light intensity stat")] private float _baseLightIntensity = 0.5f;
    [SerializeField, Tooltip("Max light intensity stat")] private float _maxIntensity = 1.0f;
    [SerializeField, Tooltip("Interpolation ratio for increasing intensity from current to max")] private float _lightIntensityLerpRatio = 0.3f;
    [SerializeField, Tooltip("Base light range stat")] private float _baseLightRange = 50f;
    [SerializeField, Tooltip("Max light range stat")] private float _maxLightRange = 130f;
    [SerializeField, Tooltip("Interpolation ratio for increasing range from current to max")] private float _lightRangeLerpRatio = 0.25f;
    private int _baseLightUpgradeCount = 0;

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
        baseData.BulletDamageMult = _baseBulletDamage;
        baseData.DamageUpgradeCount = 0;
        // fire rate
        baseData.BulletCooldown = _baseBulletCooldown;
        baseData.FireRateUpgradeCount = _baseFireRateUpgradeCount;
        // move speed
        baseData.MoveSpeed = _baseMoveSpeed;
        // light
        baseData.LightFOV = _baseLightFOV;
        baseData.LightIntensity = _baseLightIntensity;
        baseData.LightUpgradeCount = _baseLightUpgradeCount;
        baseData.LightRange = _baseLightRange;

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
                ViewManager.GetView<InGameUIView>().MaxHPUp();
                break;
            case UpgradeController.UpgradeType.Armor:
                playerData.Armor += _armorPerUpgrade;
                break;
            case UpgradeController.UpgradeType.Damage:
                playerData.BulletDamageMult *= (1 + _bulletDamagePerUpgrade);
                playerData.DamageUpgradeCount++;
                break;
            case UpgradeController.UpgradeType.FireSpeed:
                playerData.BulletCooldown *= _bulletCooldownUpgradeFactor;
                playerData.FireRateUpgradeCount++;
                break;
            case UpgradeController.UpgradeType.MoveSpeed:
                playerData.MoveSpeed += _moveSpeedPerUpgrade;
                break;
            case UpgradeController.UpgradeType.Light:
                playerData.LightFOV = Mathf.Lerp(playerData.LightFOV, _maxLightFOV, _lightFOVLerpRatio);
                playerData.LightIntensity = Mathf.Lerp(playerData.LightIntensity, _maxIntensity, _lightIntensityLerpRatio);
                playerData.LightRange = Mathf.Lerp(playerData.LightRange, _maxLightRange, _lightRangeLerpRatio);
                playerData.LightUpgradeCount++;
                break;
        }

        return playerData;
    }

    public int getMoveSpdUpgradeCount()
    {
        GameManager.Stats playerData = GameManager.Instance.PlayerData;
        return Mathf.RoundToInt((playerData.MoveSpeed- _baseMoveSpeed) / _moveSpeedPerUpgrade);
    }

}
