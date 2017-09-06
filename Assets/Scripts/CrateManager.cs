using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateManager : MonoBehaviour {

    private int _crateCount;

    public void AddCrate() {
        _crateCount++;
    }

    public void ResetCrateCount() {
        _crateCount = 0;
    }

    public int CrateCount {
        get { return _crateCount; }
    }
}
