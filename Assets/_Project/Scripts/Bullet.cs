using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    bool collided;
    public GameObject impactPrefab;
    public float lifeTime;
    private void Start() {
        //StartCoroutine(DestroyWithDelay());
    }
    
    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag != "bullet" && other.gameObject.tag != "Player" && !collided){
            collided = true;
            var impact = Instantiate( impactPrefab, other.contacts[0].point, Quaternion.identity) as GameObject;
            Destroy(impact, 2);
            Destroy(gameObject);
        }
    }

    IEnumerator DestroyWithDelay(){
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
    
}
