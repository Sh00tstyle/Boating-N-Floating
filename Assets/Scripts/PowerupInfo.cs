using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupInfo : MonoBehaviour {

	public enum Type { None, FasterReload, DamageBoost, MovementSpeedBoost, TemporaryBonusHP, AutoHPRegen, ScattershotWeapon, BarrelBombweapon };
    public Type powerupType;

    public float duration;

    public float reloadTimeFactor;
    public float damageBoostFactor;
    public float movementSpeedBoostFactor;
    public float bonusHP;
    public float regenerationAmountPerSec;
}
