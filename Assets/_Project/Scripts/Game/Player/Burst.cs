using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burst : MonoBehaviour
{
    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag != "bullet" && other.gameObject.tag != "Player"){
            Debug.Log("Bullet collided with " + other.gameObject.name);
    
            if(other.gameObject.tag == "Enemy"){
                other.gameObject.GetComponent<Enemy>().Hit();
            }
            else if(other.gameObject.tag == "Spawner"){
                other.gameObject.GetComponent<SpawnMod>().Hit();
            }
        }
    }
}
