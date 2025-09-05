using System.Collections;
using DG.Tweening;
using Nato;
using UnityEngine;
using UnityEngine.UI;

public class SkullBoss : BossBase
{
    private Sequence patternSequence;
    public bool followTarget;
    public bool stop;

    [SerializeField] private AnimationClip idleClip;
    [SerializeField] private AnimationClip jumpClip;
    [SerializeField] private AnimationClip shootClip;

    [SerializeField] private float speed = 10f;

    [SerializeField] private Bullet enemyBulletPrefab;
    [SerializeField] private Transform mouthBarrel;
    [SerializeField] private Transform leftHandBarrel;
    [SerializeField] private Transform rightHandBarrel;

    [Header("Spike Ball Settings")]
    [SerializeField] private GameObject spikeBallPrefab;
    [SerializeField] private int numberOfBalls = 8;
    [SerializeField] private float spawnRadius = 2f;
    [SerializeField] private float force = 10f;
    [SerializeField] private float upwardForce = 5f;

    [Header("References")]
    [SerializeField] private Transform bossCenter;

    [SerializeField] private AudioSource shootAudioSource;
    [SerializeField] private AudioClip shootAudio;




    public override void Start()
    {
        base.Start();

    }

    public override void StartBoss()
    {
        base.StartBoss();
        SetPatterns();
        DoPatterns();
    }

    public override void SetPatterns()
    {
        base.SetPatterns();
        patterns.Add(FollowPattern);
        patterns.Add(JumpAttackPattern);
        patterns.Add(ShootAttackPattern);
    }

    private void FollowPattern()
    {
        followTarget = true;
        patternSequence = DOTween.Sequence();
        patternSequence.AppendInterval(5f);
        patternSequence.OnComplete(() =>
        {
            followTarget = false;
            Action(1f);
        });
    }

    private void JumpAttackPattern()
    {
        stop = true;
        patternSequence = DOTween.Sequence();
        patternSequence.AppendInterval(1f);
        patternSequence.AppendCallback(() =>
        {
            stop = false;
            PlayAnimation(jumpClip);

        });
        patternSequence.AppendInterval(jumpClip.length);
        patternSequence.OnComplete(() =>
        {
            Action(1f);
        });
    }

    private void ShootAttackPattern()
    {
        stop = true;
        patternSequence = DOTween.Sequence();
        patternSequence.AppendInterval(1f);
        patternSequence.AppendCallback(() =>
        {
            stop = false;
            PlayAnimation(shootClip);
            StartCoroutine(ShootBulletsCoroutine(mouthBarrel, quantity: 15));
        });
        patternSequence.AppendInterval(1f);
        patternSequence.OnComplete(() =>
        {
            PlayAnimation(idleClip);
            Action(1f);
        });
    }

    private IEnumerator ShootBulletsCoroutine(Transform barrel, int quantity)
    {
        yield return new WaitForSeconds(0.09f);
        for (int i = 0; i < quantity; i++)
        {
            Vector3 direction = (Target.position - barrel.position).normalized;
            Vector3 noise = new Vector3(
      Random.Range(-0.5f, 0.5f),
      Random.Range(-0.5f, 0.5f),
      Random.Range(-0.1f, 0.1f)
  );
            Vector3 firePoint = barrel.position + noise;
            InstantiateProjectile(firePoint, direction);
            yield return new WaitForSeconds(0.09f);
        }
    }

    void InstantiateProjectile(Vector3 firePoint, Vector3 direction)
    {
        shootAudioSource.clip = shootAudio;
        shootAudioSource.Play();
        Bullet bullet = Instantiate(enemyBulletPrefab, firePoint, Quaternion.LookRotation(direction)).GetComponent<Bullet>();
        bullet.Setup(direction.normalized);
    }

    public void SpawnSpikeBalls()
    {
        if (spikeBallPrefab == null || bossCenter == null) return;

        for (int i = 0; i < numberOfBalls; i++)
        {
            float angle = i * Mathf.PI * 2 / numberOfBalls;

            Vector3 spawnPos = bossCenter.position + new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * spawnRadius;

            GameObject spikeBall = Instantiate(spikeBallPrefab, spawnPos, Quaternion.identity);

            Vector3 dir = (spawnPos - bossCenter.position).normalized;

            Rigidbody rb = spikeBall.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 forceDir = dir * force + Vector3.up * upwardForce;
                rb.AddForce(forceDir, ForceMode.Impulse);
            }
        }
    }

    private void Update()
    {
        if (followTarget)
        {
            Vector3 position = Body.linearVelocity;
            position.y = 0;

            Vector3 direction = Target.position - transform.position;
            direction.y = 0;

            //Body.linearVelocity = Vector3.Lerp(Vector3.zero, playerDistance, playerDistance.magnitude) * modifier * multiplier; 
            Body.linearVelocity = Vector3.Slerp(position, direction, speed * Time.deltaTime);
        }

        if (stop)
        {
            Body.linearVelocity = Vector3.Lerp(Body.linearVelocity, Vector3.zero, speed * Time.deltaTime);
        }
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

    public override void Hit()
    {
        base.Hit();
        if (Health <= 0)
            patternSequence.Kill();
    }
}
