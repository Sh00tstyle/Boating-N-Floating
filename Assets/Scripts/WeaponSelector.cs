using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSelector : MonoBehaviour {

	private enum Weapon { Cannonball, Clustershot, Barrelbomb };
    private Weapon _activeWeapon;

    private CannonballSpawnerPlayer[] _cannonballSpawner;
    private BombSpawner _bombSpawner;

    private CameraShifting _camShifting;

    public void Awake() {
        _activeWeapon = Weapon.Cannonball;

        _cannonballSpawner = GetComponentsInChildren<CannonballSpawnerPlayer>();
        _bombSpawner = GetComponentInChildren<BombSpawner>();

        _camShifting = GetComponentInChildren<CameraShifting>();
    }

    public void Update() {
        if(Input.GetKeyDown(KeyCode.U)) {
            UseWeapon();
        } else if(Input.GetKeyDown(KeyCode.I)) {
            SwitchWeapon();
        }
    }

    private void UseWeapon() {
        switch(_activeWeapon) {
            case Weapon.Cannonball:
                for(int i = 0; i < _cannonballSpawner.Length; i++) {
                    if(_camShifting.IsRight() && _cannonballSpawner[i].isRight || !_camShifting.IsRight() && !_cannonballSpawner[i].isRight) {
                        _cannonballSpawner[i].Fire();
                    }
                }
                break;

            case Weapon.Clustershot:
                //Nothing yet
                break;

            case Weapon.Barrelbomb:
                _bombSpawner.DropBomb();
                break;

            default:
                break;
        }
    }

    private void SwitchWeapon() {
        switch(_activeWeapon) {
            case Weapon.Cannonball:
                _activeWeapon = Weapon.Clustershot;
                break;

            case Weapon.Clustershot:
                _activeWeapon = Weapon.Barrelbomb;
                break;

            case Weapon.Barrelbomb:
                _activeWeapon = Weapon.Cannonball;
                break;

            default:
                break;
        }
    }

}
