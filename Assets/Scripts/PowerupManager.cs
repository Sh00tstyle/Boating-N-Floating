using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour {

    public GameObject spawnLocationHolder;
    public GameObject[] powerupContainer;

    private Transform[] _spawnLocations;

    public void Awake() {
        _spawnLocations = spawnLocationHolder.GetComponentsInChildren<Transform>();
    }

    public void SpawnPowerupAtIndex(int spawnLocationIndex, int powerupIndex) {
        GameObject newPowerup = Instantiate(powerupContainer[powerupIndex]);
        newPowerup.transform.position = _spawnLocations[spawnLocationIndex].position;
    }

    public void SpawnPowerupAtMaxDistance(Vector3 yourPosition, int powerupIndex) {
        if (powerupIndex < 0 || powerupIndex >= powerupContainer.Length) return;

        float maxDistance = 0;
        int spawnIndex = 0;

        for (int i = 0; i < _spawnLocations.Length; i++) {
            Vector3 deltaVec = _spawnLocations[i].position - yourPosition;

            if (maxDistance < deltaVec.sqrMagnitude) {
                maxDistance = deltaVec.sqrMagnitude;
                spawnIndex = i;
            }
        }

        SpawnPowerupAtIndex(spawnIndex, powerupIndex);
    }

    public void SpawnPowerupAtMinDistance(Vector3 yourPosition, int powerupIndex) {
        if (powerupIndex < 0 || powerupIndex >= powerupContainer.Length) return;

        float minDistance = 0;
        int spawnIndex = 0;

        for (int i = 0; i < _spawnLocations.Length; i++) {
            Vector3 deltaVec = _spawnLocations[i].position - yourPosition;

            if (minDistance > deltaVec.sqrMagnitude) {
                minDistance = deltaVec.sqrMagnitude;
                spawnIndex = i;
            }
        }

        SpawnPowerupAtIndex(spawnIndex, powerupIndex);
    }
}
