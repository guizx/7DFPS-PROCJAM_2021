using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    LevelController levelController;
    public float health, invulDur;
    public Slider slider;
    bool invul;
    public static bool dead;

    public GameObject hurtEffectUI;
    public float hurtEffectDuration;

    public UnityEvent OnHurt;
    public UnityEvent OnDied;

    private void Start()
    {
        levelController = GameObject.Find("Level").GetComponent<LevelController>();
        slider.maxValue = health;
        slider.value = health;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Enemy" && !invul && !dead)
        {
            StartCoroutine(Invulnerable());
            StartCoroutine(HurtEffectCoroutine());
            Damage(0.1f);
            OnHurt?.Invoke();
        }

        else if (other.gameObject.tag == "Boss" && !invul && !dead)
        {
            StartCoroutine(Invulnerable());
            StartCoroutine(HurtEffectCoroutine());
            Damage(0.1f);
            OnHurt?.Invoke();
        }
        else if (other.gameObject.tag == "DeathPit" && !dead)
        {
            Die();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && !invul && !dead)
        {
            StartCoroutine(Invulnerable());
            StartCoroutine(HurtEffectCoroutine());
            Damage(0.1f);
            OnHurt?.Invoke();
        }
    }

    IEnumerator Invulnerable()
    {
        invul = true;
        yield return new WaitForSeconds(invulDur);
        invul = false;
    }

    void Damage(float dmg)
    {
        slider.value = health;
        health -= dmg;
        if (health <= 0) Die();
    }
    void Die()
    {
        if (!dead)
        {            
            dead = true;
            levelController.GameOver();
            OnDied?.Invoke();
        }
    }

    private IEnumerator HurtEffectCoroutine()
    {
        hurtEffectUI.SetActive(true);
        yield return new WaitForSeconds(hurtEffectDuration);
        hurtEffectUI.SetActive(false);
    }
}
