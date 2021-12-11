using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Repulsion : MonoBehaviour
{
    Boid boid;
    public float radius;
    public float repulsionForce;
    Boid[] boids;
    Vector3 average;
    int found;
    // Start is called before the first frame update
    void Start()
    {
        boid = GetComponent<Boid>();
    }

    // Update is called once per frame
    void Update()
    {
        boids = FindObjectsOfType<Boid>();
        average = Vector3.zero;
        found = 0;

        foreach (var boid in boids.Where(b => b != boid))
        {
            var diff = boid.transform.position - this.transform.position;
            if(diff.magnitude < radius){
                average += diff;
                found += 1;
            }
        }

        if(found >0){
            average = average / found;
            boid.velocity -= Vector3.Lerp(Vector3.zero, average, average.magnitude / radius) * repulsionForce;
        }
    }
}
