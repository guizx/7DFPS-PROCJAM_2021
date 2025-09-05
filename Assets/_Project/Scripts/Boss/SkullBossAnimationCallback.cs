using UnityEngine;

public class SkullBossAnimationCallback : MonoBehaviour
{
    private SkullBoss SkullBoss;
    [SerializeField] AudioSource audioSource;
    private void Awake()
    {
        SkullBoss = GetComponentInParent<SkullBoss>();
    }

    public void SpawnSpikes()
    {
        SkullBoss.SpawnSpikeBalls();
    }

    public void StartBoss()
    {
        SkullBoss.StartBoss();
    }

    public void PlayAudio(AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }
}
