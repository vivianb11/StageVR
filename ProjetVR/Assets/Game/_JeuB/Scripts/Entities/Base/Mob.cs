using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace JeuB
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(BoxCollider))]
    public abstract class Mob : Entity
    {
        [Header("Mob Characteristics")]
        public int scoreOnDeath;

        [Header("On Hit Parameters")]
        [SerializeField] float knockCooldown;
        [SerializeField] GameObject hpLossParticles;
        [SerializeField] GameObject deathParticles;
        [SerializeField] Outline outlineEffect;

        [Header("Sounds")]
        [SerializeField] Sound[] sounds;

        [SerializeField] UnityEvent onKnocked = new UnityEvent();

        FeedbackScale feedbackScale;

        protected bool _isKnocked = false;
        private Tween _tween;
        private bool isHit = false;

        private int currentMultiplier;

        private Transform _shield;

        protected override void EntityStart()
        {
            OnDamaged.AddListener(Damaged);

            _shield = FindObjectOfType<ShieldManager>().transform;

            _tween = GetComponent<Tween>();

            target.GetComponent<ProtectedToothBehaviours>().onDeath.AddListener(Freeze);
        }

        protected override void EntityUpdate()
        {
            Vector3 toothDirection = (transform.position - _shield.position).normalized;
            float angleToTooth = Vector3.Angle(_shield.forward, toothDirection);

            outlineEffect.enabled = angleToTooth < 30f;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform == target) Attack(other.gameObject);
        }

        private IEnumerator Knocked()
        {
            _isKnocked = true;
            onKnocked.Invoke();
            moveSpeed *= 2f;
            yield return new WaitForSeconds(knockCooldown);
            moveSpeed *= 0.5f;
            _isKnocked = false;
            isHit = false;
        }

        public override void Move()
        {
            transform.LookAt(target.position, transform.parent.up);

            if (_isKnocked)
            {
                transform.Translate(Vector3.back * moveSpeed * Time.deltaTime, Space.Self);
                return;
            }

            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime, Space.Self);
        }

        private void Damaged()
        {
            if (isHit) return;
            isHit = true;

            GameObject instantiatedObject = Instantiate(hpLossParticles, transform.position, transform.rotation);
            Destroy(instantiatedObject, 3f);

            if (Health > 0) StartCoroutine(Knocked());
        }

        private void Attack(GameObject protectedTooth)
        {
            var tempToothBehaviours = protectedTooth.GetComponent<ProtectedToothBehaviours>();

            tempToothBehaviours.Damaged();
            OnDeath?.Invoke();
            OnDeath.RemoveListener(tempToothBehaviours.ScoreMultiplier);

            Kill();
        }

        public void Freeze()
        {
            moveSpeed = 0f;
            Invoke(nameof(MissileShoot), 1.75f);
        }

        public void MissileShoot()
        {
            _tween.tweenMontages[0].tweenProperties[0].toObject = target.transform;
            _tween.PlayTween("Missile");
        }

        public override void Kill()
        {
            GameObject instantiatedObject = Instantiate(deathParticles, transform.position, transform.rotation);

            var tempToothBehaviours = target.GetComponent<ProtectedToothBehaviours>();
            tempToothBehaviours.enemyPoints = scoreOnDeath;
            OnDeath.AddListener(tempToothBehaviours.ScoreMultiplier);

            Destroy(instantiatedObject, 3f);
            Destroy(gameObject);
        }
    }
}
