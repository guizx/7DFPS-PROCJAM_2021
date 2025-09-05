using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : Modifier
{
    public Rigidbody myRb;
    public EmissionMod myEmissionMod;
    public GameObject playerObj, deathParticle, birthParticle;
    public SpawnMod mySpawner;
    public float orbitRadius, gravityVelocity, findPlayerRate, health;
    Vector3 playerDistance, gravityVector;

    public UnityEvent OnDied;

    public AudioClip dieClip;

    public static System.Action OnEnemyDied;

    // Start is called before the first frame update
    void Start()
    {
        LevelController.OnBossSpawned += HandleOnBossSpawned;
        var birthp = Instantiate(birthParticle, transform.position, Quaternion.identity);
        Destroy(birthp, 2);
        myRb = GetComponent<Rigidbody>();
        playerObj = GameObject.Find("Player");
        //gravityVector = new Vector3(0, gravityVelocity, 0);
        InvokeRepeating("FindPlayer", 1.0f, findPlayerRate);
    }

    void OnDestroy()
    {
        LevelController.OnBossSpawned -= HandleOnBossSpawned;

    }

    private void HandleOnBossSpawned()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerObj.transform.position), Time.deltaTime / modifier * multiplier);
        if (playerObj == null)
            return;
        transform.LookAt(playerObj.transform);
        myRb.linearVelocity = Vector3.Lerp(Vector3.zero, playerDistance, playerDistance.magnitude) * modifier * multiplier; ;//
        //myRb.MovePosition(transform.position + playerDistance.normalized * Time.deltaTime * speed);
    }

    void FindPlayer()
    {
        if (playerObj == null) return;
        playerDistance = playerObj.transform.position - this.transform.position;
        playerDistance = new Vector3(playerDistance.x + Random.Range(-orbitRadius, orbitRadius), playerDistance.y, playerDistance.z + Random.Range(-orbitRadius, orbitRadius));
        //this.transform.LookAt(playerDistance);
    }

    public void Hit()
    {
        //myEmissionMod.TweenColor();
        //health--;
        //if(health == 0) Die();
        Die();
    }

    void Die()
    {
        OnEnemyDied?.Invoke();
        mySpawner.EnemyDie();
        var deathp = Instantiate(deathParticle, transform.position, Quaternion.identity);
        OnDied?.Invoke();
        InstantiateAudio(dieClip, new Vector2(0.95f, 1f));
        Destroy(deathp, 2);
        Destroy(gameObject);
    }

    public void InstantiateAudio(AudioClip clip, Vector2 randomPitch)
    {
        GameObject audioObject = new GameObject("AudioPlayer");
        AudioSource source = audioObject.AddComponent<AudioSource>();

        source.clip = clip;
        source.playOnAwake = false;
        source.loop = false;
        float randomPitchValue = Random.Range(randomPitch.x, randomPitch.y);
        source.pitch = randomPitchValue;

        source.Play();

        Destroy(audioObject, clip.length);
    }
}
