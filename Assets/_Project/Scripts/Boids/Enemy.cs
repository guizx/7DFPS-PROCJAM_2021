using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Modifier
{
    public Rigidbody myRb;
    public GameObject playerObj;
    public float orbitRadius, gravityVelocity, findPlayerRate;
    Vector3 playerDistance, gravityVector;

    // Start is called before the first frame update
    void Start()
    {
        myRb = GetComponent<Rigidbody>();
        playerObj = GameObject.Find("Player");
        gravityVector = new Vector3(0, gravityVelocity, 0);
        InvokeRepeating("FindPlayer", 0.0f, findPlayerRate);
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerObj.transform.position), Time.deltaTime);
        myRb.velocity = Vector3.Lerp(Vector3.zero, playerDistance, playerDistance.magnitude) * modifier * multiplier;
        //myRb.MovePosition(transform.position + playerDistance.normalized * Time.deltaTime * speed);
    }

    void FindPlayer(){
        playerDistance = playerObj.transform.position - this.transform.position;
        playerDistance = new Vector3(playerDistance.x + Random.Range(-orbitRadius, orbitRadius), playerDistance.y, playerDistance.z + Random.Range(-orbitRadius, orbitRadius));
        //this.transform.LookAt(playerDistance);
    }
}
