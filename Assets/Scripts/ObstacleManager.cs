using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour {
    [SerializeField]
    private List<GameObject> obstacles;

    public void Awake() {
        obstacles = new List<GameObject>();

        Transform[] transforms = GetComponentsInChildren<Transform>();

        for (int i = 0; i < transforms.Length; i++) {
            if (transforms[i].gameObject.tag == "Obstacle") RegisterObstacle(transforms[i].gameObject);
        }
    }

    public void RegisterObstacle(GameObject crate) {
        if (!obstacles.Contains(crate)) obstacles.Add(crate);
    }

    public void RemoveObstacle(GameObject crate) {
        if (obstacles.Contains(crate)) obstacles.Remove(crate);
    }

    public GameObject GetClosestObstacle(Vector3 position) {
        GameObject closestObstacle = null;
        float distance = float.MaxValue;

        for (int i = 0; i < obstacles.Count; i++) {
            Vector3 deltaVec = obstacles[i].transform.position - position;

            if (closestObstacle == null || deltaVec.sqrMagnitude < distance) {
                closestObstacle = obstacles[i];
                distance = deltaVec.sqrMagnitude;
            }
        }

        return closestObstacle;
    }

    public List<GameObject> Obstacles {
        get { return obstacles; }
    }
}

