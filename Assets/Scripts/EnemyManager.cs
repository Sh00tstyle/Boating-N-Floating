using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    private int _enemyStartAmt;

    public int EnemyAmt {
        get {
            int counter = 0;

            Transform[] enemies = GetComponentsInChildren<Transform>();

            for(int i = 0; i < enemies.Length; i++) {
                if(enemies[i].gameObject.activeSelf  && enemies[i].gameObject.tag == Tags.ENEMY) counter++;
            }

            return counter;
        }
    }

    public int EnemyStartAmt {
        get {
            if(_enemyStartAmt == 0) {
                Transform[] enemies = GetComponentsInChildren<Transform>();

                for (int i = 0; i < enemies.Length; i++) {
                    if (enemies[i].gameObject.tag == Tags.ENEMY) _enemyStartAmt++;
                }
            }

            return _enemyStartAmt;
        }
    }
}
