using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingAI : MonoBehaviour {

    private AISpawner spawner;
    private Rigidbody rb;
    private Vector3 velocity;

    [Header("Steering")]
    public float MaxSpeed;
    public float MinSpeed;
    public float MaxSteeringForce;

    public float AllignmentFactor;
    public float AllignmentRange;
    public float CohesionFactor;
    public float CohesionRange;
    public float SeperationFactor;
    public float SeperationRange;

    public int TargetFactor;
    public float RotationSpeed;

    [HideInInspector]
    public ObstacleManager ObstacleManager;
    [Header("Obstacle Avoidance")]
    public float AvoidanceFactor;
    public float MaxSeeAhead;
    public float AheadAngles;
    public float AheadAnglesFactor;
    public float MaxAvoidanceForce;
    public float BoundaryMultiplier;
    public float CollisionCooldownTime;
    private float timeSinceLastCollision;
    private Vector3 avoidanceForce;

    public bool debugMode = false;
    private bool fireMode;

    private Vector3 Target;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        fireMode = false;
	}

    private void Update() {
        UpdateVelocity();
    }

    // Update is called once per frame
    private void UpdateVelocity () {
        //velocity = Vector3.zero;
        if (!fireMode) {
            Vector3 allignment = AllignmentFactor * calculateAlignment();
            Vector3 cohesion = CohesionFactor * calculateCohesion();
            Vector3 seperation = SeperationFactor * calculateSeperation();

            velocity += allignment;
            velocity += cohesion;
            velocity += seperation;

            //velocity.Normalize();
            //velocity *= Time.deltaTime;

            if (timeSinceLastCollision <= 0) {
                Vector3 avoidance = AvoidanceFactor * collisionAvoidance(velocity);
                if (avoidance != Vector3.zero) {
                    avoidanceForce = avoidance;
                    timeSinceLastCollision = CollisionCooldownTime;
                }
                else {
                    avoidanceForce = Vector3.zero;
                }
            }

            if (timeSinceLastCollision > 0) {
                velocity += avoidanceForce;
            }

            velocity.y = 0;

            restrictVelocity();

            rb.velocity = velocity;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(velocity), Time.deltaTime * RotationSpeed);
            //transform.rotation = Quaternion.LookRotation(velocity);

            if (debugMode) {
                Debug.DrawRay(transform.position, velocity, Color.white);
                Debug.DrawRay(transform.position, allignment, Color.green);
                Debug.DrawRay(transform.position, cohesion, Color.blue);
                Debug.DrawRay(transform.position, seperation, Color.red);
                Debug.DrawRay(transform.position, avoidanceForce, Color.black);
            }

            if (timeSinceLastCollision > 0) {
                timeSinceLastCollision -= Time.deltaTime;
                if (timeSinceLastCollision < 0) timeSinceLastCollision = 0;
            }
        }
        else {
            print("Preparing to fire");
        }

    }

    private void restrictVelocity() {
        if (velocity.magnitude > MaxSpeed) velocity = velocity.normalized * MaxSpeed;
        if (velocity.magnitude < MinSpeed) velocity = velocity.normalized * MinSpeed;
    }

    private Vector3 calculateAlignment() {
        Vector3 averageVelocity = Vector3.zero;
        int count = 0;

        foreach(GameObject agent in spawner.agents) {
            Vector3 agentVel = agent.GetComponent<FlockingAI>().velocity;
            Vector3 distance = agent.transform.position - transform.position;

            if ((distance.magnitude > 0) && (distance.magnitude < AllignmentRange)) {
                averageVelocity += agentVel;
                count++;
            }
        }

        Vector3 targetDistance = Target - transform.position;
        averageVelocity += targetDistance.normalized * (MaxSpeed * TargetFactor);

        averageVelocity /= count + TargetFactor;

        // As long as the vector is greater than 0
        if (averageVelocity.magnitude > 0) {
            // Implement Reynolds: Steering = Desired - Velocity
            averageVelocity.Normalize();
            averageVelocity *= MaxSpeed;
            Vector3 steer = averageVelocity - velocity;
            if (steer.magnitude > MaxSteeringForce) steer = steer.normalized * MaxSteeringForce;
            return steer;
        }
        else {
            return Vector3.zero;
        }
    }

    private Vector3 calculateCohesion() {
        Vector3 averagePosition = Vector3.zero;
        int numberOfNeighbors = 0;

        Vector3 closestAgent = Vector3.zero;

        foreach (GameObject agent in spawner.agents) {
            Vector3 agentPos = agent.transform.position;
            Vector3 distance = agentPos - transform.position;
            if (closestAgent == Vector3.zero) closestAgent = agentPos;

            Vector3 distanceToClosest = closestAgent - transform.position;
            if(distance.magnitude < CohesionRange && distance.magnitude > 0) {
                averagePosition += agentPos;
                numberOfNeighbors++;

            }           
            if (distance.magnitude < distanceToClosest.magnitude) closestAgent = agentPos;
        }

        if (numberOfNeighbors > 0) {
            averagePosition /= numberOfNeighbors;
            return seek(averagePosition);
        }
        else {
            if (spawner.agents.Length > 0) return seek(closestAgent);
            else return Vector3.zero;
        }
    }

    public Vector3 calculateLocalSwarmCenter() {
        Vector3 averagePosition = Vector3.zero;
        int numberOfNeighbors = 0;

        foreach (GameObject agent in spawner.agents) {
            Vector3 agentPos = agent.transform.position;
            Vector3 distance = agentPos - transform.position;

            if (distance.magnitude < CohesionRange && distance.magnitude > 0) {
                averagePosition += agentPos;
                numberOfNeighbors++;
            }
        }

        if (numberOfNeighbors > 0) {
            averagePosition /= numberOfNeighbors;
            return averagePosition;
        }
        else {
            Vector3 globalSwarmCenter = Vector3.zero;
            foreach (GameObject agent in spawner.agents) {
                globalSwarmCenter += agent.transform.position;
            }
            globalSwarmCenter /= spawner.FlockSize;
            return globalSwarmCenter;
        }
    }

    private Vector3 calculateSeperation() {

        Vector3 steer = new Vector3(0, 0, 0);
        int count = 0;

        // For every agent check if it's too close
        foreach(GameObject agent in spawner.agents) {
            Vector3 distance = agent.transform.position - transform.position;

            // If the distance is greater than 0 and less than an arbitrary amount (0 when you are yourself)
            if ((distance.magnitude > 0) && (distance.magnitude < SeperationRange)) {

                // Calculate vector pointing away from neighbor
                Vector3 diff = transform.position - agent.transform.position;
                diff.Normalize();
                diff /= distance.magnitude;// Weight by distance
                steer += diff;
                count++; 
            }
        }
        // Average -- divide by how many
        if (count > 0) {
            steer /= count;
        }

        // As long as the vector is greater than 0
        if (steer.magnitude > 0) {
            // Implement Reynolds: Steering = Desired - Velocity
            steer.Normalize();
            steer *= MaxSpeed;
            steer -= velocity;
            if (steer.magnitude > MaxSteeringForce) steer = steer.normalized * MaxSteeringForce;
        }
        return steer;
    }

    private Vector3 seek(Vector3 target) {
        Vector3 desired = target - transform.position;  // A vector pointing from the position to the target
                                                          
        desired.Normalize();
        desired *= MaxSpeed;

        // Steering = Desired minus Velocity
        Vector3 steer = desired - velocity;
        if (steer.magnitude > MaxSteeringForce) steer = steer.normalized * MaxSteeringForce;  // Limit to maximum steering force
        return steer;
    }

    //TODO: Check sides before turning
    private Vector3 collisionAvoidance(Vector3 velocity)     {
        //print("Velocity.magnitude; " + velocity.magnitude);
        float divider1 = velocity.magnitude - MinSpeed;
        float divider2 = MinSpeed - MaxSpeed;

        float percentage = divider1 / divider2;
        Mathf.Clamp01(percentage);
        //print("Percentage: " + percentage);
        float dynamicLength = 1f * Mathf.Abs(percentage);
        //print("Dynamic Length: " + dynamicLength);

        Vector3 ahead = (velocity.normalized * dynamicLength * MaxSeeAhead);
        Vector3 ahead2 = (velocity.normalized * dynamicLength * MaxSeeAhead) * 0.5f;

        Vector3 aheadLeft = Quaternion.Euler(0, -AheadAngles, 0) * ahead;
        Vector3 aheadLeft2 = aheadLeft * 0.5f;
        Vector3 aheadRight = Quaternion.Euler(0, AheadAngles, 0) * ahead;
        Vector3 aheadRight2 = aheadRight * 0.5f;

        //Sponsored by Bram den Hond aka. Lifesaver
        ahead += transform.position;
        ahead2 += transform.position;
        aheadLeft += transform.position;
        aheadLeft2 += transform.position;
        aheadRight += transform.position;
        aheadRight2 += transform.position;

        Debug.DrawLine(transform.position, ahead, Color.yellow);
        Debug.DrawLine(transform.position, aheadLeft, Color.yellow);
        Debug.DrawLine(transform.position, aheadRight, Color.yellow);

        Vector3 avoidanceForce = Vector3.zero;

        Vector3 mostThreatening = findMostThreateningObstacle(ahead, ahead2);
        //print("Most Threatening: " + mostThreatening);
        Vector3 mostThreateningLeft = findMostThreateningObstacle(aheadLeft, aheadLeft2);
        Vector3 mostThreateningRight = findMostThreateningObstacle(aheadRight, aheadRight2);

        if(mostThreatening != Vector3.zero) {
            avoidanceForce = ahead - mostThreatening;
            avoidanceForce.Normalize();
            avoidanceForce *= MaxAvoidanceForce;
            //Vector3 returnProduct = avoidanceForce - 2 * (Vector3.Dot(avoidanceForce, velocity.normalized)) * velocity.normalized;
            //Debug.DrawLine(transform.position, returnProduct, Color.magenta);
            //return returnProduct;
            return avoidanceForce;
        }
        else if(mostThreateningLeft != Vector3.zero) {
            avoidanceForce = (aheadLeft - mostThreateningLeft) * AheadAnglesFactor;
            avoidanceForce.Normalize();
            avoidanceForce *= MaxAvoidanceForce;
            //Vector3 returnProduct = avoidanceForce - 2 * (Vector3.Dot(avoidanceForce, velocity.normalized)) * velocity.normalized;
            //Debug.DrawLine(transform.position, returnProduct, Color.magenta);
            //return returnProduct;
            return avoidanceForce;
        }
        else if (mostThreateningRight != Vector3.zero) {
            avoidanceForce = (aheadLeft - mostThreateningRight) * AheadAnglesFactor;
            avoidanceForce.Normalize();
            avoidanceForce *= MaxAvoidanceForce;
            //Vector3 returnProduct = avoidanceForce - 2 * (Vector3.Dot(avoidanceForce, velocity.normalized)) * velocity.normalized;
            //Debug.DrawLine(transform.position, returnProduct, Color.magenta);
            //return returnProduct;
            return avoidanceForce;
        }
        else {
            return Vector3.zero;
        }
    }

    //Finds the most relevent aka nearest obstacle for the given ahead vectors
    private Vector3 findMostThreateningObstacle(Vector3 ahead, Vector3 ahead2) {
        Vector3 mostThreatening = Vector3.zero;
        
        //For all the objets
        for (int i = 0; i < ObstacleManager.Obstacles.Count; i++) {
            GameObject obstacle = ObstacleManager.Obstacles[i];

            if (!obstacle.GetComponent<SphereCollider>()) continue;

            //Calculate "collision sphere" radius
            SphereCollider obstacleCollider = obstacle.GetComponent<SphereCollider>();
            float obstacleRadius =  (obstacleCollider.radius * Mathf.Max(obstacle.transform.lossyScale.x, obstacle.transform.lossyScale.z)) * BoundaryMultiplier;
           //Check if it collides with it
            bool collision = intersectsWithObstacleRange(ahead, ahead2, obstacle.transform.position, obstacleRadius);
            //If it will collide and mostThreating is null or the deltaVec is shorter
            if (collision && (mostThreatening == Vector3.zero || (transform.position - obstacle.transform.position).magnitude < (transform.position - mostThreatening).magnitude)) {
                mostThreatening = obstacle.transform.position;
            }
        }
        return mostThreatening;
    }

    //Checks if the ahead vetors collide with the given obstacle
    private bool intersectsWithObstacleRange(Vector3 ahead, Vector3 ahead2, Vector3 nearestObstacle,float obstacleRadius) {
        return distance(ahead, nearestObstacle) < obstacleRadius || distance(ahead2, nearestObstacle) < obstacleRadius;
    }

    private float distance(Vector3 a, Vector3 b)  {
        return Mathf.Sqrt((a.x - b.x) * (a.x - b.x)  + (a.z - b.z) * (a.z - b.z));
    }

    private void goToFirePosition() {

    }

    public Vector3 Velocity {
        get { return velocity; }
        set { velocity = value; }
    }

    public bool FireMode {
        get { return fireMode; }
        set { fireMode = value; }
    }

    public void SetSpawner(AISpawner sp) {
        spawner = sp;
    }

    public void SetTarget(Vector3 target) {
        Target = target;
    }
}
