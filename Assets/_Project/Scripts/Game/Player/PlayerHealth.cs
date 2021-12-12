using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    LevelController levelController;
    private void Start() {
        levelController = GameObject.Find("Level").GetComponent<LevelController>();
    }
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
        levelController.GameOver();
    }
}
