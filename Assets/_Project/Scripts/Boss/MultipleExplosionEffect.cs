
using UnityEngine;
using UnityEngine.Events;

namespace Nato
{
    public class MultipleExplosionEffect : MonoBehaviour
    {
        public float durationAux;
        private float timeCounter = 0;
        public float Duration = 5f;
        public float DelayFinish = 0f;
        [SerializeField] private float period = 0.3f;
        [SerializeField] private GameObject explosionEffectPrefab;
        [SerializeField] private AudioClip explosionSound;
        private bool canExplode = false;
        [SerializeField] private Transform explosionCenter;

        [SerializeField] private float range = 1;

        public UnityEvent OnExplosionShowed;
        public UnityEvent OnExplosionUpdated;
        public UnityEvent OnExplosionFinished;
        public System.Action callback;

        private void Awake()
        {
            durationAux = Duration;

        }

        private void Update()
        {
            if (!canExplode)
                return;

            Duration -= Time.deltaTime;
            if (Duration <= 0)
            {
                canExplode = false;
                OnExplosionFinished?.Invoke();
                callback?.Invoke();
            }

            timeCounter -= Time.deltaTime;
            if (timeCounter <= 0 && Duration > DelayFinish)
            {
                OnExplosionShowed?.Invoke();
                OnExplosionUpdated?.Invoke();
                Instantiate(explosionEffectPrefab, explosionCenter.position + (Random.insideUnitSphere * range), Quaternion.identity);
                timeCounter = period;
            }
        }

        public void DoMultipleExplosions()
        {
            Duration = durationAux;
            canExplode = true;
        }

        public void DoMultipleExplosions(System.Action callback)
        {
            Duration = durationAux;
            canExplode = true;
            this.callback = callback;
        }

        public void SetExplosionCenter(Transform explosionCenter)
        {
            this.explosionCenter = explosionCenter;
        }

    }
}

