using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Contain : MonoBehaviour
{
    Boid boid;
    public float radius;
    // Start is called before the first frame update
    void Start()
    {
        boid = GetComponent<Boid>();
    }

    // Update is called once per frame
    void Update()
    {
        if(boid.transform.position.magnitude > radius){
        boid.velocity += this.transform.position.normalized * (radius - boid.transform.position.magnitude) * Time.deltaTime;
        }
    }
}
