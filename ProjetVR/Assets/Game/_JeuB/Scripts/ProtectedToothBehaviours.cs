using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace JeuB
{
    public class ProtectedToothBehaviours : MonoBehaviour
    {
        [Header("Tooth Characteristics")]
        [SerializeField] int Maxhealth;
        [SerializeField] int receivedDamagedOnHit;
        [SerializeField] UnityEvent onDamaged = new UnityEvent();
        [SerializeField] Material material2HP; //the material the tooth has at 2 hp
        [SerializeField] Material material1HP; //the material the tooth has at 1 hp

        [SerializeField] float shakeMagnitude = 0.1f;
        [SerializeField] float shakeSpeed = 50f;
        [SerializeField] float shakeDuration = 3f;
        [SerializeField] int enemiesToKillForMultiplier;
        [SerializeField] int MaxMultiplier;
        [SerializeField] GameObject multiplierTexts;
        private bool scoreEnabled = true;
        public bool ScoreEnabled {
            get { return scoreEnabled; }
            set { scoreEnabled = value; }
        }

        private int killStreak = 0;
        public int multiplier = 1;
        public int enemyPoints;

        private Vector3 originalPosition;

        public UnityEvent onDeath = new UnityEvent();
        public UnityEvent onExplosion = new UnityEvent();
        public GameObject toothExplosion;
        private OutlineScale _outlineScaleEffect;
        private Outline _outlineEffect;
        private float _originalWidth = 5f;
        private float _finalWidth = 10f;
        private float _currentWidth;
        private float _speed = 0.5f;

        private int health;

        //variables for thomas trying to make outline work
        public float minValueOutline = 6f;
        public float maxValueOutline = 12f;
        public float duration = 1f;

        private float outlineValue; //the variable that constantly changes up and down
        private bool increasing = true;
        private float timer = 0f;

        [SerializeField] GameObject _lock;

        void OnEnable()
        {
            originalPosition = transform.position;
            _outlineScaleEffect = GetComponent<OutlineScale>();
            _outlineEffect = GetComponent<Outline>();
            _outlineEffect.OutlineWidth = 0;

            health = Maxhealth;
            HeadMotionTracker.Instance.Excited.AddListener(() => ScoreEnabled = false);
            HeadMotionTracker.Instance.Excited.AddListener(() => _lock.SetActive(true));
            HeadMotionTracker.Instance.Normal.AddListener(() => ScoreEnabled = true);
            HeadMotionTracker.Instance.Normal.AddListener(() => _lock.SetActive(false));
        }

        public void Damaged()
        {
            if (health == 0)
                return;

            health = Mathf.Clamp(health - receivedDamagedOnHit, 0, Maxhealth);
            onDamaged.Invoke();
            killStreak = 0;
            multiplier = 1;
        
            foreach(Transform child in multiplierTexts.transform)
            {
                child.gameObject.SetActive(false);
            }

            if (health == 0)
            {
                onDeath.Invoke();

                Invoke(nameof(Explode), 2.05f);
                GameManager.Instance.ReloadGameMode(3);
                //StartCoroutine(ShakeAndDie());
            }
        }

        public void OutlinePulsating()
        {
            timer += Time.deltaTime;

            if (timer >= duration)
            {
                timer = 0f;
                increasing = !increasing;
            }

            if (increasing)
            {
                outlineValue = Mathf.Lerp(minValueOutline, maxValueOutline, timer / duration);
            }
            else
            {
                outlineValue = Mathf.Lerp(maxValueOutline, minValueOutline, timer / duration);
            }

            _outlineEffect.OutlineWidth = outlineValue;
        }

        void Update()
        {
            if (health == 1)
            {
                OutlinePulsating();
            }
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

        public void ScoreMultiplier()
        {
            if (!scoreEnabled) return;

            int textIndex;
            killStreak++;
            if (killStreak % enemiesToKillForMultiplier == 0)
            {
                if (multiplier < MaxMultiplier)
                {
                    multiplier++;

                    if (multiplier > 1)
                    {
                        textIndex = multiplier - 2;
                        Transform selectedText = multiplierTexts.transform.GetChild(textIndex);
                        selectedText.gameObject.SetActive(true);

                        if (multiplier == 2)
                        {
                        
                        }
                        else
                        {
                            Transform deletedText = multiplierTexts.transform.GetChild(textIndex - 1);
                            deletedText.gameObject.SetActive(false);
                        }

                    }

                }
            }
            ScoreManager.Instance.AddScore(enemyPoints * multiplier);
        }
    }
}
