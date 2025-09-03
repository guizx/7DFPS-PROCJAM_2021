using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Boid : MonoBehaviour
{
    public Vector3 velocity;
    public Rigidbody myRb;
    public GameObject playerObj, containOrigin;
    public bool alignment, cohesion, repulsion, contain, aggro;
    public float maxVelocity, gravityVelocity, containRadius, alignmentRadius, cohesionRadius, repulsionRadius, aggroRadius, alignmentForce, cohesionForce, repulsionForce;
    Boid myBoid;
    Boid[] boids;
    Vector3 alignmentAverage, repulsionAverage, cohesionAverage, aggroDistance, containDistance, boidDistance, gravityVector; //
    int alignmentFound, repulsionFound, cohesionFound;

    //public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        myBoid = GetComponent<Boid>();
        myRb = GetComponent<Rigidbody>();
        playerObj = GameObject.Find("Player");
        containOrigin = GameObject.Find("Origin");
        gravityVector = new Vector3(0, gravityVelocity, 0);
        //player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        boids = FindObjectsOfType<Boid>();
        alignmentAverage = Vector3.zero;
        cohesionAverage = Vector3.zero;
        repulsionAverage = Vector3.zero;
        boidDistance = Vector3.zero;
        aggroDistance = Vector3.zero;
        alignmentFound = 0;
        cohesionFound = 0;
        repulsionFound = 0;

        foreach (var boid in boids.Where(b => b != myBoid))
        {
            boidDistance = boid.transform.position - this.transform.position;

            if(boidDistance.magnitude < alignmentRadius){
                alignmentAverage += boid.velocity;
                alignmentFound += 1;
            }
            if(boidDistance.magnitude < cohesionRadius){
                cohesionAverage += boidDistance;
                cohesionFound += 1;
            }
            if(boidDistance.magnitude < repulsionRadius){
                repulsionAverage += boidDistance;
                repulsionFound += 1;
            }
        }
        if(alignmentFound > 0 && alignment){
            alignmentAverage = alignmentAverage / alignmentFound;
            velocity += Vector3.Lerp(velocity, alignmentAverage, Time.deltaTime) * alignmentForce;; //
        }

        if(cohesionFound >0 && cohesion){
            cohesionAverage = cohesionAverage / cohesionFound;
            velocity += Vector3.Lerp(Vector3.zero, cohesionAverage, cohesionAverage.magnitude / cohesionRadius) * cohesionForce;; //
        }

        if(repulsionFound >0 && repulsion){
            repulsionAverage = repulsionAverage / repulsionFound;
            velocity -= Vector3.Lerp(Vector3.zero, repulsionAverage, repulsionAverage.magnitude / repulsionRadius)* repulsionForce; //
        }

        containDistance = containOrigin.transform.position - this.transform.position;
        if(containDistance.magnitude > containRadius  && contain){
        velocity += this.transform.position.normalized * (containRadius - transform.position.magnitude) * Time.deltaTime;
        }

        if(velocity.magnitude > maxVelocity){
            velocity = velocity.normalized * maxVelocity;
        }

        myRb.linearVelocity += velocity * Time.deltaTime;

        aggroDistance = playerObj.transform.position - this.transform.position;
        
        if(aggroDistance.magnitude < aggroRadius && aggro){
            this.transform.LookAt(playerObj.transform);
            myRb.linearVelocity = Vector3.Lerp(Vector3.zero, aggroDistance, aggroDistance.magnitude);
        }

        //fake gravity
        myRb.AddForce( gravityVector * Time.deltaTime, ForceMode.Acceleration);
    }
}
