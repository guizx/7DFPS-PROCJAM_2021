using System.Collections;
using System.Collections.Generic;
using Nato;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    bool collided;
    public GameObject impactPrefab, spawnImpactPrefab;
    public float lifeTime, speed;
    Vector3 shootDirection;
    public bool isPlayerBullet = true;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Setup(Vector3 direction)
    {
        Destroy(gameObject, lifeTime);
        shootDirection = direction;
    }

    private void Update()
    {
        //transform.position += shootDirection * speed * Time.deltaTime;
        
                    rb.MovePosition(rb.position + shootDirection * speed * Time.fixedDeltaTime);

    }
    
    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag != "bullet" && other.gameObject.tag != "Player" && !collided){
            collided = true;
            Debug.Log("Bullet collided with " + other.gameObject.name);
            var impact = Instantiate( impactPrefab, other.contacts[0].point, Quaternion.identity) as GameObject;
            Destroy(impact, 2);
            Destroy(gameObject);
            if(other.gameObject.tag == "Enemy"){
                other.gameObject.GetComponent<Enemy>()?.Hit();
            }
            else if(other.gameObject.tag == "Boss"){
                other.gameObject.GetComponent<BossBase>()?.Hit();
            }
            else if (other.gameObject.tag == "Spawner")
            {
                var spawnImpact = Instantiate(spawnImpactPrefab, other.contacts[0].point, Quaternion.identity) as GameObject;
                Destroy(spawnImpact, 2);
                other.gameObject.GetComponent<SpawnMod>().Hit();
            }
        }
    }
}
