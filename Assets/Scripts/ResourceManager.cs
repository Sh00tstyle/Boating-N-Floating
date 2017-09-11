using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour {

    private int _goldAmount;
    private int _bombAmount;

    public void Awake() {
        _goldAmount = 150;
    }

    public void GainGold(int amount) {
        _goldAmount += amount;
    }

    public bool SpendGold(int amount) {
        if (_goldAmount - amount < 0) return false;

        _goldAmount -= amount;
        return true;
    }

    public int GoldAmount {
        get { return _goldAmount;  }
    }
}
