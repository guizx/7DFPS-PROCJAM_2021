using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    bool collided;
    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "Enemy" && !collided){
            collided = true;
            Die();
        }
        else if(other.gameObject.tag == "DeathPit" && !collided){
            collided = true;
            Die();
        }
    }
    void Die(){
        Scene scene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(scene.name);
    }
}
