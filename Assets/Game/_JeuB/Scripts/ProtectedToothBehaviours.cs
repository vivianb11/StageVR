using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace JeuB
{
    public class ProtectedToothBehaviours : MonoBehaviour
    {
        [Header("Tooth Characteristics")]
        [SerializeField] int maxHealth;
        private int health;
        [SerializeField] int receivedDamagedOnHit;


        [Space(10)]
        [Header("Shake Effect")]
        [SerializeField] float shakeMagnitude = 0.1f;
        [SerializeField] float shakeSpeed = 50f;
        [SerializeField] float shakeDuration = 3f;


        [Space(10)]
        [Header("Score Multiplier")]
        [SerializeField] int enemiesToKillForMultiplier;
        [SerializeField] int MaxMultiplier;
        [SerializeField] GameObject multiplierTexts;

        private bool scoreEnabled = true;
        private bool Invincible
        {
            get
            {
                return JeuBCommands.listenersActiveStats["Invincible"];
            }
            set
            {
                JeuBCommands.SetObject("Invincible", value);
            }
        }

        private int killStreak = 0;
        public int multiplier = 1;

        [Space(10)]
        [Header("Tooth Explosion S&F")]
        public GameObject toothExplosion;

        [Space(10)]
        [Header("Outline S&F")]
        [SerializeField] Color color2HP;
        [SerializeField] Color color1HP;
        private OutlineScale _outlineScaleEffect;
        private Outline _outlineEffect;
        public float minValueOutline = 6f;
        public float maxValueOutline = 12f;
        public float duration = 1f;
        private float outlineValue;
        private bool increasing = true;
        private float timer = 0f;
        [SerializeField] GameObject _lock;


        [Space(10)]
        [Foldout("Events")][SerializeField] UnityEvent onDamaged = new UnityEvent();
        [Foldout("Events")] public UnityEvent onDeath = new UnityEvent();
        [Foldout("Events")] public UnityEvent onExplosion = new UnityEvent();
        [Foldout("Events")] public UnityEvent onHeal = new UnityEvent();

        private Vector3 originalPosition;

        void OnEnable()
        {
            originalPosition = transform.position;
            _outlineScaleEffect = GetComponent<OutlineScale>();
            _outlineEffect = GetComponent<Outline>();
            _outlineEffect.OutlineWidth = 0;

            health = maxHealth;

            HeadMotionTracker.Instance.Excited.AddListener(() => scoreEnabled = false);
            HeadMotionTracker.Instance.Excited.AddListener(() => _lock.SetActive(true));
            HeadMotionTracker.Instance.Normal.AddListener(() => scoreEnabled = true);
            HeadMotionTracker.Instance.Normal.AddListener(() => _lock.SetActive(false));

            if (!JeuBCommands.listeners.ContainsKey("Invincible")) JeuBCommands.AddKey("Invincible", null);
        }

        private void OnDestroy()
        {
            HeadMotionTracker.Instance.Excited.RemoveAllListeners();
            HeadMotionTracker.Instance.Normal.RemoveAllListeners();
        }

        public void Damaged()
        {
            if (health == 0 || Invincible) return;

            health = Mathf.Clamp(health - receivedDamagedOnHit, 0, maxHealth);
            onDamaged.Invoke();
            killStreak = 0;
            multiplier = 1;

            foreach (Transform child in multiplierTexts.transform) child.gameObject.SetActive(false);

            _outlineEffect.OutlineColor = (health == 2) ? color2HP : color1HP;

            if (health != 0) return;
            onDeath.Invoke();
            Invoke(nameof(Explode), 2.05f);
            GameManager.Instance.ReloadGameMode(GameManager.ReloadCause.DEATH, 3);
        }

        public void Heal(int amount)
        {
            if (health == 0) return;

            health = Mathf.Clamp(health + amount, 0, maxHealth);
            onHeal.Invoke();

            _outlineEffect.OutlineColor = (health == 2) ? color2HP : (health == 1) ? color1HP : Color.clear;
        }

        public void SetInvincible(bool value)
        {
            Invincible = value;
        }

        public void OutlinePulsating()
        {
            timer += Time.deltaTime;

            if (timer >= duration)
            {
                timer = 0f;
                increasing = !increasing;
            }

            if (increasing) outlineValue = Mathf.Lerp(minValueOutline, maxValueOutline, timer / duration);

            else outlineValue = Mathf.Lerp(maxValueOutline, minValueOutline, timer / duration);

            _outlineEffect.OutlineWidth = outlineValue;
        }

        void Update()
        {
            if (health == 1 || health == 2) OutlinePulsating();
        }

        IEnumerator ShakeAndDie()
        {
            float elapsedTime = 0f;

            while (elapsedTime < shakeDuration)
            {
                Vector3 shakeOffset = Random.insideUnitSphere * shakeMagnitude;
                transform.position = originalPosition + shakeOffset;
                elapsedTime += Time.deltaTime * shakeSpeed;
                yield return null;
            }

            yield return null;
        }

        private void Explode()
        {
            onExplosion.Invoke();
            Instantiate(toothExplosion, transform.parent); //the tooth explodes into many pieces
            gameObject.SetActive(false);
        }

        public void ScoreMultiplier(int points)
        {
            Debug.Log("ScoreMultiplier" + scoreEnabled);

            if (!scoreEnabled) return;

            int textIndex;
            killStreak++;

            if (killStreak % enemiesToKillForMultiplier != 0)
            {
                ScoreManager.Instance.AddScore(points * multiplier);
                return;
            }

            if (multiplier < MaxMultiplier)
            {
                multiplier++;

                if (multiplier < 1)
                {
                    ScoreManager.Instance.AddScore(points * multiplier);
                    return;
                }

                textIndex = multiplier - 2;
                Transform selectedText = multiplierTexts.transform.GetChild(textIndex);
                selectedText.gameObject.SetActive(true);

                if (multiplier != 2)
                {
                    Transform deletedText = multiplierTexts.transform.GetChild(textIndex - 1);
                    deletedText.gameObject.SetActive(false);
                }
            }
            ScoreManager.Instance.AddScore(points * multiplier);
        }
    }
}
