using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpawnerController : Modifier
{
    public SpawnMod spawnerPrefab;
    public List<SpawnMod> spawners;
    public List<Transform> availableSpawnPoints, takenSpawnPoints;
    public List<int> spawnIndexes;
    public float threshold, initialDelay, spawnDelay;

    public bool wait = true;
    // Start is called before the first frame update
    void Start()
    {
        //SpawnMod instance = Instantiate(spawnerPrefab, spawnPoints[0]);
        //instance.Initialize(0);
        LeanTween.init(800);
        StartCoroutine(Wait());

        LevelController.OnBossSpawned += HandleOnBossSpawned;
    }

    private void OnDestroy()
    {
        LevelController.OnBossSpawned -= HandleOnBossSpawned;
    }

    private void HandleOnBossSpawned()
    {
        enabled = false;
        gameObject.SetActive(false);
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(initialDelay);
        wait = false;
    }

    // Update is called once per frame
    void Update()
    {
        Modify();
        if (spawners.Count < 8 && !wait) CheckBands();
    }
    void CheckBands()
    {
        for (int i = 0; i < 8; i++)
        {
            if (allModifiers[i] > threshold && !spawnIndexes.Contains(i)) StartCoroutine(Spawn(i));
        }
    }

    IEnumerator Spawn(int index)
    {
        wait = true;
        yield return new WaitForSeconds(spawnDelay);
        spawnIndexes.Add(index);
        Transform spawnPoint = availableSpawnPoints[Random.Range(0, availableSpawnPoints.Count - 1)];
        availableSpawnPoints.Remove(spawnPoint);
        takenSpawnPoints.Add(spawnPoint);
        SpawnMod instance = Instantiate(spawnerPrefab, spawnPoint, false);
        spawners.Add(instance);
        instance.Initialize(index, spawnPoint);
        wait = false;
    }

    public void SpawnDeath(SpawnMod spawner, int index, Transform spawnPoint)
    {
        spawners.Remove(spawner);
        spawnIndexes.Remove(index);
        takenSpawnPoints.Remove(spawnPoint);
        availableSpawnPoints.Add(spawnPoint);
    }

    public void SetSpawner(SpawnMod spawner)
    {
        spawnerPrefab = spawner;
    }
}
