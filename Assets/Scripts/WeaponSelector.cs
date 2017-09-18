using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSelector : MonoBehaviour {

	public enum Weapon { Cannonball, Scattershot, Barrelbomb };

    private CannonballSpawnerPlayer[] _cannonballSpawner;
    private BombSpawner _bombSpawner;

    private CameraShifting _camShifting;

    public void Awake() {
        _cannonballSpawner = GetComponentsInChildren<CannonballSpawnerPlayer>();
        _bombSpawner = GetComponentInChildren<BombSpawner>();

        _camShifting = GetComponentInChildren<CameraShifting>();
    }

    public void Update() {
        if(Input.GetKeyDown(KeyCode.Space)) {
            UseWeapon(Weapon.Cannonball);
        }
    }

    public void UseWeapon(Weapon weaponType) {
        CameraShifting.Direction shootingDirection = _camShifting.CurrentDirection;

        switch (weaponType) {
            case Weapon.Cannonball:
                for(int i = 0; i < _cannonballSpawner.Length; i++) {
                    if(shootingDirection == CameraShifting.Direction.Left && !_cannonballSpawner[i].isRight) _cannonballSpawner[i].SetCannonballDelay();
                    else if(shootingDirection == CameraShifting.Direction.Right && _cannonballSpawner[i].isRight) _cannonballSpawner[i].SetCannonballDelay();
                    else if(shootingDirection == CameraShifting.Direction.Front) _cannonballSpawner[i].SetCannonballDelay();
                }
                break;

            case Weapon.Scattershot:
                for (int i = 0; i < _cannonballSpawner.Length; i++) {
                    if (shootingDirection == CameraShifting.Direction.Left && !_cannonballSpawner[i].isRight) _cannonballSpawner[i].SetClustshotDelay();
                    else if (shootingDirection == CameraShifting.Direction.Right && _cannonballSpawner[i].isRight) _cannonballSpawner[i].SetClustshotDelay();
                    else if (shootingDirection == CameraShifting.Direction.Front) _cannonballSpawner[i].SetClustshotDelay();
                }
                break;

            case Weapon.Barrelbomb:
                _bombSpawner.DropBomb();
                break;

            default:
                break;
        }
    }
}