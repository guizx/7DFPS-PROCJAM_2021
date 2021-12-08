using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMod : Modifier
{
    public Transform spawnPoint;
    public Enemy enemy;
    public float enemyVel, threshold, delay = 0.5f;
    public int enemyCount, enemyLimit;
    bool wait;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(modifier > threshold && !wait && enemyCount < enemyLimit){
            StartCoroutine(Spawn());
        }
    }

    IEnumerator Spawn(){
        wait = true;
        Enemy enemyInstance = Instantiate(enemy.gameObject, spawnPoint.position, Quaternion.identity, spawnPoint).GetComponent<Enemy>();
        enemyInstance.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-enemyVel, enemyVel), Random.Range(-enemyVel, enemyVel), Random.Range(-enemyVel, enemyVel));
        enemyCount++;
        yield return new WaitForSeconds(delay);
        wait = false;
    }
}
