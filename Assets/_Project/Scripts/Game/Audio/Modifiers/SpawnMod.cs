using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMod : Modifier
{
    public SpawnerController spawnerController;
    public LevelController level;
    public Transform mySpawnPoint, enemySpawnPoint;
    public Enemy enemy;
    public Enemy[] enemies;
    public Renderer myRend;
    public Material[] materials;
    public GameObject birthParticle;
    public EmissionMod emissionMod;
    public float health, enemyVel, threshold, delay = 0.5f;
    public int enemyCount, enemyLimit;
    bool wait;
    // Start is called before the first frame update
    public void Initialize(int range, Transform spawnPoint)
    {
        level = GameObject.Find("Level").GetComponent<LevelController>();

        wait = true;
        mySpawnPoint = spawnPoint;
        spawnerController = GameObject.Find("SpawnerController").GetComponent<SpawnerController>();
        rangeToFollow = (Range)range;
        GetComponent<ScaleMod>().rangeToFollow = rangeToFollow;
        emissionMod = GetComponentInChildren<EmissionMod>();
        emissionMod.rangeToFollow = rangeToFollow;
        
        var birthp = Instantiate(birthParticle, transform, false);
        Destroy(birthp, 2);

        LeanTween.moveY(gameObject, 0, 1.0f).setOnComplete(DontWait);

        //refactor this later
        switch (range)
        {
            case 0:
            enemy = enemies[0];
            myRend.material = materials[0];
            break;
            case 1:
            case 2:
            enemy = enemies[1];
            myRend.material = materials[1];
            break;
            case 3:
            case 4:
            case 5:
            enemy = enemies[2];
            myRend.material = materials[2];
            break;
            case 6:
            case 7:
            enemy = enemies[3];
            myRend.material = materials[3];
            break;
        }
    }

    void DontWait(){
        wait = false;
    }

    // Update is called once per frame
    void Update()
    {
        Modify();
        if(modifier > threshold && !wait && enemyCount < enemyLimit){
            StartCoroutine(Spawn());
        }
    }

    IEnumerator Spawn(){
        wait = true;
        Enemy enemyInstance = Instantiate(enemy.gameObject, enemySpawnPoint.position, Quaternion.identity).GetComponent<Enemy>();
        enemyInstance.rangeToFollow = rangeToFollow;
        enemyInstance.mySpawner = GetComponent<SpawnMod>();
        //enemyInstance.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-enemyVel, enemyVel), Random.Range(-enemyVel, enemyVel), Random.Range(-enemyVel, enemyVel));
        enemyCount++;
        yield return new WaitForSeconds(delay);
        wait = false;
    }

    public void Hit(){
        emissionMod.TweenColor();
        health--;
        if(health == 0) Die();
    }

    void Die(){
        level.AddScore(100);
        var deathp = Instantiate(birthParticle, transform, false);
        spawnerController.SpawnDeath(this, (int)rangeToFollow, mySpawnPoint);
        LeanTween.moveY(gameObject, -15.0f, 1.0f).setOnComplete(FinishDeath);
        Destroy(deathp, 2);
    }

    void FinishDeath(){
        Destroy(gameObject);
    }

    public void EnemyDie(){
        level.AddScore(10);
        enemyCount--;
    }
}
