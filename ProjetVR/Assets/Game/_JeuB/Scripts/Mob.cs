using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace JeuB
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(BoxCollider))]
    public class Mob : MonoBehaviour
    {
        [Header("Mob Characteristics")]
        [ReadOnly] public Transform target;
        [SerializeField] int lifepoints;
        [ReadOnly] public float moveSpeed = 0;
        [SerializeField] bool isKnockable;
        public bool isBonus;
        public int scoreOnDeath;
    
        public bool canRotate;

        [Header("On Hit Parameters")]
        [SerializeField] int receivedDamagedOnHit;
        [ShowIf("isKnockable")] [SerializeField] float knockCooldown;
        [ShowIf("isKnockable")] [SerializeField] Outline outlineEffect;
        [ShowIf("isKnockable")] [SerializeField] GameObject hpLossParticles;
        [SerializeField] GameObject deathParticles;

        [Header("Rotation Parameters")]
        [ShowIf("canRotate")] public int[] degree;

        [Header("Sounds")]
        [SerializeField] Sound[] sounds;

        [SerializeField] UnityEvent onKnocked = new UnityEvent();
        [SerializeField] UnityEvent onDeath = new UnityEvent();

        FeedbackScale feedbackScale;

        private bool _isKnocked = false;
        private Tween _tween;
        private bool isHit = false;

        private int currentMultiplier;

        public ProtectedToothBehaviours protectedToothBehaviours;

        private Transform _shield;

        private void Start()
        {
            _shield = FindObjectOfType<ShieldManager>().transform;

            _tween = GetComponent<Tween>();
            if (! canRotate) return;
        
            GetComponent<BoxCollider>().isTrigger = true;
            StartCoroutine(DelayRotation());
        }

        private void Update()
        {
            Move();
            RotateMesh();

            Vector3 toothDirection = (transform.position - _shield.position).normalized;
            float angleToTooth = Vector3.Angle(_shield.forward, toothDirection);

            outlineEffect.enabled = angleToTooth < 30f;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Shield")) Damaged();
            if (other.gameObject.CompareTag("Protected")) Attack(other.gameObject);
        }

        private void RotateMesh()
        {
            if (!canRotate) transform.GetChild(0).transform.Rotate(0, 0, 50 * Time.deltaTime);
        }

        private IEnumerator DelayRotation()
        {
            float timeBeforeRotation = Random.Range(2.5f, 3f);
            yield return new WaitForSeconds(timeBeforeRotation);
            yield return Rotation();
        }

        private IEnumerator Rotation()
        {
            _isKnocked = true;
            float elapsedTime = 0f;
            float rotationDuration = 2f;
            int RandomRotationIndex = Random.Range(0, degree.Length);
            Quaternion startRotation = transform.parent.localRotation;
            Quaternion targetRotation = Quaternion.Euler(0f, degree[RandomRotationIndex], 0f);

            while (elapsedTime < rotationDuration)
            {
                float t = elapsedTime / rotationDuration;
                transform.parent.localRotation = Quaternion.Lerp(startRotation, targetRotation, t);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            //transform.parent.localRotation = targetRotation;
            _isKnocked = false;
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

        private void Move()
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
            Invoke("MissileShoot", 1.75f);
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
    }
}
