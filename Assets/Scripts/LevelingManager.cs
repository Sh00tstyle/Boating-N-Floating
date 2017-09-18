using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelingManager : MonoBehaviour {

    public float[] expPerLevel;

    private int _currentLevel;

    private float _currentExp;

    public void GainExp(float exp) {
        _currentExp += exp;

        if (_currentExp >= expPerLevel[_currentLevel - 1]) LevelUp();
    }

    private void LevelUp() {
        _currentExp = 0f;

        if (_currentLevel <= expPerLevel.Length) _currentLevel++;

        //Do other stuff, like updating UI and giving the player boni
    }
}
