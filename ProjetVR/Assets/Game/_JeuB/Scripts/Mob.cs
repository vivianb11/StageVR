using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace JeuB
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(BoxCollider))]
    public class Mob : Entity
    {
        [Header("Mob Characteristics")]
        [SerializeField] int lifepoints;
        [SerializeField] bool isKnockable;
        public bool isBonus;
        public int scoreOnDeath;

        [Header("On Hit Parameters")]
        [SerializeField] int receivedDamagedOnHit;
        [ShowIf("isKnockable")] [SerializeField] float knockCooldown;
        [ShowIf("isKnockable")] [SerializeField] GameObject hpLossParticles;
        [SerializeField] GameObject deathParticles;
        [SerializeField] Outline outlineEffect;

        [Header("Sounds")]
        [SerializeField] Sound[] sounds;

        [SerializeField] UnityEvent onKnocked = new UnityEvent();
        [SerializeField] UnityEvent onDeath = new UnityEvent();

        FeedbackScale feedbackScale;

        protected bool _isKnocked = false;
        private Tween _tween;
        private bool isHit = false;

        private int currentMultiplier;

        public ProtectedToothBehaviours protectedToothBehaviours;

        private Transform _shield;

        protected override void EntityStart()
        {
            _shield = FindObjectOfType<ShieldManager>().transform;

            _tween = GetComponent<Tween>();
        }

        protected override void EntityUpdate()
        {
            Move();

            Vector3 toothDirection = (transform.position - _shield.position).normalized;
            float angleToTooth = Vector3.Angle(_shield.forward, toothDirection);

            outlineEffect.enabled = angleToTooth < 30f;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Shield")) Damaged();
            if (other.gameObject.CompareTag("Protected")) Attack(other.gameObject);
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
               if (isKnockable) transform.Translate(Vector3.back * moveSpeed * Time.deltaTime, Space.Self);
                return;
            }
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime, Space.Self);
        }

        private void Damaged()
        {
            if (isHit) return;
            isHit = true;
            lifepoints -= receivedDamagedOnHit;
            if (IsDeadCheck())
            {
                DamageParticles();
                return;
            }
            gameObject.GetComponent<FeedbackScale>().ScaleIn();
            gameObject.GetComponent<FeedbackScale>().ScaleOut();
            if (!isKnockable) return;
            if (!IsDeadCheck()) StartCoroutine(Knocked());
            //ChangeOutline();
        }

        private void Attack(GameObject protectedTooth)
        {
            var tempToothBehaviours = protectedTooth.GetComponent<ProtectedToothBehaviours>();
            if (isBonus) 
            {
                tempToothBehaviours.enemyPoints = scoreOnDeath;
                onDeath.AddListener(tempToothBehaviours.ScoreMultiplier);
            }
            else tempToothBehaviours.Damaged();
            onDeath.Invoke();
            onDeath.RemoveListener(tempToothBehaviours.ScoreMultiplier);
            Destroy(gameObject);
        }

        private bool IsDeadCheck()
        {
            bool condition = lifepoints == 0;
            if (condition)
            {
                onDeath.Invoke();
                //currentMultiplier = protectedToothBehaviours.multiplier;
                //Debug.Log(currentMultiplier);
                //ScoreManager.Instance.AddScore(scoreOnDeath * currentMultiplier);
                Destroy(gameObject);
            }
            return condition;
        }

        private void ChangeOutline()
        {
            if (lifepoints == 2)  outlineEffect.OutlineWidth = 2;
            else if (lifepoints == 1) outlineEffect.enabled = false;
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

        public void DamageParticles()
        {
            if (lifepoints >= 1)
            {
                GameObject instantiatedObject = Instantiate(hpLossParticles, transform.position, transform.rotation);
                Destroy(instantiatedObject, 3f);
            }

            else
            {
                GameObject instantiatedObject = Instantiate(deathParticles, transform.position, transform.rotation);
                Destroy(instantiatedObject, 3f);
            }
        }

        public override void Kill()
        {
            throw new System.NotImplementedException();
        }
    }
}
