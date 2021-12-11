using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Aggressiveness : MonoBehaviour
{
    Boid boid;
    public float radius, aggro;
    public GameObject player;
    
    // Start is called before the first frame update
    void Start()
    {
        boid = GetComponent<Boid>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        var diff = boid.transform.position - this.transform.position;
        //boid.velocity += Vector3.Lerp(Vector3.zero, diff, diff.magnitude * aggro);
        this.transform.LookAt(player.transform);
        boid.velocity += Vector3.Lerp(Vector3.zero, diff, diff.magnitude / radius) * Time.deltaTime;
    }
}
