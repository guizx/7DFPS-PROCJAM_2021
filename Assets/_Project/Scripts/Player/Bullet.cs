using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    bool collided;
    public GameObject impactPrefab;
    public float lifeTime, speed;
    Vector3 shootDirection;
    public void Setup(Vector3 direction) {
        Destroy(gameObject, lifeTime);
        shootDirection = direction;
    }

    private void Update() {
        transform.position += shootDirection * speed * Time.deltaTime;
    }
    
    private void OntriggerEnter(Collision other) {
        if(other.gameObject.tag != "bullet" && other.gameObject.tag != "Player" && !collided){
            collided = true;
            //Debug.Log(other.gameObject.name);
            var impact = Instantiate( impactPrefab, other.contacts[0].point, Quaternion.identity) as GameObject;
            Destroy(impact, 2);
            Destroy(gameObject);
            if(other.gameObject.tag == "Enemy"){
                Destroy(other.gameObject);
            }
        }
    }
}
