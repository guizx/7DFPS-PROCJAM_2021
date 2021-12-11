using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Modifier
{
    public Rigidbody myRb;
    public EmissionMod myEmissionMod;
    public GameObject playerObj, deathParticle, birthParticle;
    public SpawnMod mySpawner;
    public float orbitRadius, gravityVelocity, findPlayerRate, health;
    Vector3 playerDistance, gravityVector;

    // Start is called before the first frame update
    void Start()
    {
        var birthp = Instantiate(birthParticle, transform.position, Quaternion.identity);
        Destroy(birthp, 2);
        myRb = GetComponent<Rigidbody>();
        playerObj = GameObject.Find("Player");
        //gravityVector = new Vector3(0, gravityVelocity, 0);
        InvokeRepeating("FindPlayer", 1.0f, findPlayerRate);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerObj.transform.position), Time.deltaTime / modifier * multiplier);
        transform.LookAt(playerObj.transform);
        myRb.velocity = Vector3.Lerp(Vector3.zero, playerDistance, playerDistance.magnitude) * modifier * multiplier; ;//
        //myRb.MovePosition(transform.position + playerDistance.normalized * Time.deltaTime * speed);
    }

    void FindPlayer(){
        playerDistance = playerObj.transform.position - this.transform.position;
        playerDistance = new Vector3(playerDistance.x + Random.Range(-orbitRadius, orbitRadius), playerDistance.y, playerDistance.z + Random.Range(-orbitRadius, orbitRadius));
        //this.transform.LookAt(playerDistance);
    }

    public void Hit(){
        //myEmissionMod.TweenColor();
        //health--;
        //if(health == 0) Die();
        Die();
    }

    void Die(){
        mySpawner.EnemyDie();
        var deathp = Instantiate(deathParticle, transform.position, Quaternion.identity);
        Destroy(deathp, 2);
        Destroy(gameObject);
    }
}
