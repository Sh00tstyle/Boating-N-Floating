using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupActivationScript : MonoBehaviour {

    private CannonballSpawnerPlayer[] _cannonballSpawners;
    private BombSpawner _bombSpawner;
    private ShipMovement _shipMovement;
    private HealthScript _healthScript;

    private WeaponSelector _weaponSelector;

    private float _powerupTimer;
    private PowerupInfo.Type _activePowerup;

    public void Awake() {
        _cannonballSpawners = GetComponentsInChildren<CannonballSpawnerPlayer>();
        _bombSpawner = GetComponentInChildren<BombSpawner>();
        _shipMovement = GetComponent<ShipMovement>();
        _healthScript = GetComponent<HealthScript>();

        _weaponSelector = GetComponent<WeaponSelector>();
    }

    public void Update() {
        if(Input.GetKeyDown(KeyCode.LeftShift)) {
            if (_activePowerup == PowerupInfo.Type.ScattershotWeapon) _weaponSelector.UseWeapon(WeaponSelector.Weapon.Scattershot);
            else if (_activePowerup == PowerupInfo.Type.BarrelBombweapon) _weaponSelector.UseWeapon(WeaponSelector.Weapon.Barrelbomb);
        } 

        if (_powerupTimer <= 0f && _activePowerup != PowerupInfo.Type.None) DeactivatePickup();

        _powerupTimer -= Time.deltaTime;
    }

    public void ActivatePowerup(PowerupInfo powerupInfo) {
        _powerupTimer = powerupInfo.duration;
        _activePowerup = powerupInfo.powerupType;

        switch (_activePowerup) {
            case PowerupInfo.Type.FasterReload:
                for(int i = 0; i < _cannonballSpawners.Length; i++) {
                    _cannonballSpawners[i].ChangeCooldown(powerupInfo.reloadTimeFactor);
                }

                _bombSpawner.ChangeCooldown(powerupInfo.reloadTimeFactor);
                break;

            case PowerupInfo.Type.DamageBoost:
                for (int i = 0; i < _cannonballSpawners.Length; i++) {
                    _cannonballSpawners[i].ChangeDamage(powerupInfo.damageBoostFactor);
                }

                _bombSpawner.ChangeDamage(powerupInfo.reloadTimeFactor);
                break;

            case PowerupInfo.Type.MovementSpeedBoost:
                _shipMovement.ChangeMaxSpeed(powerupInfo.movementSpeedBoostFactor);
                break;

            case PowerupInfo.Type.TemporaryBonusHP:
                _healthScript.AddTempHP(powerupInfo.bonusHP);
                break;

            case PowerupInfo.Type.AutoHPRegen:
                _healthScript.ApplyRegeneration(powerupInfo.duration, powerupInfo.regenerationAmountPerSec);
                break;

            default:
                break;
        }
    }

    public void DeactivatePickup() {
        switch (_activePowerup) {
            case PowerupInfo.Type.FasterReload:
                for (int i = 0; i < _cannonballSpawners.Length; i++) {
                    _cannonballSpawners[i].ResetCooldown();
                }

                _bombSpawner.ResetCooldown();
                break;

            case PowerupInfo.Type.DamageBoost:
                for (int i = 0; i < _cannonballSpawners.Length; i++) {
                    _cannonballSpawners[i].ResetDamage();
                }

                _bombSpawner.ResetDamage();
                break;

            case PowerupInfo.Type.MovementSpeedBoost:
                _shipMovement.ResetMaxSpeed();
                break;

            case PowerupInfo.Type.TemporaryBonusHP:
                _healthScript.RemoveTempHP();
                break;

            default:
                break;
        }

        _activePowerup = PowerupInfo.Type.None;
    }
}
