//using DG.Tweening;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Nato
{
    [RequireComponent(typeof(Rigidbody))]
    public class BossBase : MonoBehaviour
    {
        [field: Header("Boss Base")]
        [field: Space(10)]
        [SerializeField] protected Vector3 initialPosition;
        [SerializeField] private bool useInitialPosition = true;
        //[field: SerializeField] public CharacterSO BossData { get; private set; }
        public Rigidbody Body { get; private set; }

        public Transform Target { get; private set; }
        [field: SerializeField] public Animator AnimatorComponent { get; private set; }
        public MultipleExplosionEffect MultipleExplosionEffect { get; private set; }

        [SerializeField] protected List<int> lifesEventList = new List<int>();
        [SerializeField] protected int lifeParts = 3;
        private bool hasHalfLife;
        protected List<UnityAction> patterns = new List<UnityAction>();
        private UnityAction lastPattern;

        protected bool freezeBoss;

        [SerializeField] protected Transform bossArtTransform;
        [SerializeField] protected GameObject fogEffectPrefab;

        [SerializeField] protected bool mapLocked;

        public System.Action OnHurtLifeChanged;

        [SerializeField] private float separationDistance = 1.5f;
        [SerializeField] private float separationStrength = 1f;
        [SerializeField] private float percentRoullette = 0.5f;

        [SerializeField] protected Collider2D bossCollider;
        [SerializeField] protected Collider2D damagePlayerCollider;
        [SerializeField] protected Collider2D[] otherColliders;



        [field: SerializeField] public Transform PivotTarget;

        public virtual void Awake()
        {
            if (useInitialPosition)
                transform.position = initialPosition;


            Body = GetComponent<Rigidbody>();
            MultipleExplosionEffect = GetComponent<MultipleExplosionEffect>();
            MultipleExplosionEffect.OnExplosionUpdated.AddListener(DieEffect);


        }

        private void DieEffect()
        {
        }

        private void OnDestroy()
        {

            MultipleExplosionEffect.OnExplosionUpdated.RemoveAllListeners();

        }

        public virtual void Hurt()
        {


        }


        public virtual void Start()
        {
            Target = GameObject.FindGameObjectWithTag("Player").transform;
        }

        public virtual void StartBoss()
        {

        }


        public virtual void SetPatterns() { }

        public virtual void DoPatterns() => Action(time: 1f);

        protected void Action(float time = 0.5f)
        {
            if (freezeBoss || !gameObject.activeInHierarchy)
                return;
            StartCoroutine(WaitToChangeAction(time));
        }

        protected IEnumerator WaitToChangeAction(float time)
        {
            yield return new WaitForSeconds(time);
            UnityAction randomAction = patterns[Random.Range(0, patterns.Count)];
            if (lastPattern != null)
            {
                while (lastPattern == randomAction)
                {
                    randomAction = patterns[Random.Range(0, patterns.Count)];
                }
                lastPattern = randomAction;
                randomAction();
            }
            else
            {
                lastPattern = randomAction;
                randomAction();
            }
        }

        public Vector2 CalculateSeparation()
        {
            Vector2 separationForce = Vector2.zero;
            int neighborCount = 0;

            // Obt�m todos os colliders pr�ximos dentro da dist�ncia de separa��o
            Collider2D[] colliders = Physics2D.OverlapCircleAll(Body.position, separationDistance);

            foreach (var collider in colliders)
            {
                if (collider.gameObject != gameObject && collider.TryGetComponent<BossBase>(out BossBase enemy))
                {
                    float distance = Vector2.Distance(transform.position, collider.transform.position);
                    if (distance < separationDistance)
                    {
                        // Dire��o oposta ao inimigo pr�ximo
                        Vector2 directionAway = (transform.position - collider.transform.position).normalized;
                        separationForce += directionAway / distance; // Inversamente proporcional � dist�ncia
                        neighborCount++;
                    }
                }
            }

            if (neighborCount > 0)
            {
                separationForce /= neighborCount;
            }

            return separationForce;
        }

        public virtual void StopActions()
        {
            StopAllCoroutines();
        }

        public virtual void Die()
        {
            if (damagePlayerCollider != null)
                damagePlayerCollider.enabled = false;
            DisableOtherColliders();

        }

        private void EnableOtherColliders()
        {
            for (int i = 0; i < otherColliders.Length; i++)
                otherColliders[i].enabled = true;

        }

        protected void DisableOtherColliders()
        {
            for (int i = 0; i < otherColliders.Length; i++)
                otherColliders[i].enabled = false;
        }

        protected void DisableCollider()
        {
            bossCollider.enabled = false;
        }

        public bool HasTarget()
        {
            return Target != null && Target.gameObject.activeInHierarchy;
        }


        public void PlayAnimation(AnimationClip animationClip)
        {
            if (gameObject.activeInHierarchy)
                AnimatorComponent.Play(animationClip.name);
        }

        public void Show(float duration, System.Action callback = null)
        {

        }










    }
}

