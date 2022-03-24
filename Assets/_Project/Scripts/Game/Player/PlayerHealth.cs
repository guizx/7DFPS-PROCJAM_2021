using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    LevelController levelController;
    public float health, invulDur;
    public Slider slider; 
    bool invul, dead;
    private void Start() {
        levelController = GameObject.Find("Level").GetComponent<LevelController>();
        slider.maxValue = health;
        slider.value = health;
    }
    
    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "Enemy" && !invul && !dead){
            StartCoroutine(Invulnerable());
            Damage(0.1f);
        }
        else if(other.gameObject.tag == "DeathPit" && !dead){
            Die();
        }
    }

    IEnumerator Invulnerable(){
        invul = true;
        yield return new WaitForSeconds(invulDur);
        invul = false;
    }

    void Damage(float dmg){
        slider.value = health;
        health -= dmg;
        if(health <= 0) Die();
    }
    void Die(){
        dead = true;
        levelController.GameOver();
    }
}
