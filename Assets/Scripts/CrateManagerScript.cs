using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateManagerScript : MonoBehaviour {

    private List<GameObject> _crateList;

    public void Awake() {
        _crateList = new List<GameObject>();

        Transform[] transforms = GetComponentsInChildren<Transform>();

        for(int i = 0; i < transforms.Length; i++) {
            if (transforms[i].gameObject.tag == Tags.CRATE) RegisterCrate(transforms[i].gameObject);
        }
    }

    public void RegisterCrate(GameObject crate) {
        if (!_crateList.Contains(crate)) _crateList.Add(crate);
    }

    public void RemoveCrate(GameObject crate) {
        if (_crateList.Contains(crate)) _crateList.Remove(crate);
    }

    public GameObject GetClosestCrate(Vector3 position) {
        if (_crateList.Count <= 0) return null;

        GameObject closestCrate = null;
        float distance = float.MaxValue;

        for(int i = 0; i < _crateList.Count; i++) {
            Vector3 deltaVec = _crateList[i].transform.position - position;

            if(closestCrate == null || deltaVec.sqrMagnitude < distance) {
                closestCrate = _crateList[i];
                distance = deltaVec.sqrMagnitude;
            }
        }

        return closestCrate;
    }

    public List<GameObject> CrateList {
        get { return _crateList; }
    }
}
