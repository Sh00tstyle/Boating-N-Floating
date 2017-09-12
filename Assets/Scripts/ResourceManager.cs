using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour {

    private int _goldAmount;
    private int _bombAmount;

    public void Awake() {
        _goldAmount = 150;
    }

    public void Update() {
        if(Input.GetKeyDown(KeyCode.M)) {
            BuyBomb(5, 0);
        }
    }

    public void GainGold(int amount) {
        _goldAmount += amount;
    }

    public bool SpendGold(int amount) {
        if (_goldAmount - amount < 0) return false;

        _goldAmount -= amount;
        return true;
    }

    public bool BuyBomb(int amount, int costs) {
        if (_goldAmount < costs) return false;

        _goldAmount -= costs;
        _bombAmount += amount;

        return true;
    }

    public void UseBomb() {
        if (_bombAmount > 0) _bombAmount--;
    }

    public int GoldAmount {
        get { return _goldAmount;  }
    }

    public int BombAmount {
        get { return _bombAmount; }
    }
    
}
