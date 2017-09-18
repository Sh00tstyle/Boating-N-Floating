using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {

    private BehaviourAI behaviour;

    public void setParent(BehaviourAI parent) {
        behaviour = parent;
    }

    public BehaviourAI Behaviour {
        get { return behaviour;  }
    }
}