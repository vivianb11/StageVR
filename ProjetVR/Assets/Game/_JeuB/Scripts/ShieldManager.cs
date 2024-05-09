using UnityEngine;

namespace JeuB
{
    public class ShieldManager : MonoBehaviour
    {
        public static ShieldManager Instance;
        //[SerializeField] UnityEvent onHit = new UnityEvent();
        //private int scoreDeathEnemy;

        public enum ShieldType { LITLLE, BIG }

        [SerializeField] GameObject _littleShield;
        [SerializeField] GameObject _bigShield;

        private void Awake()
        {
            Instance = this;
        }

        void Update()
        {
            transform.LookAt(EyeManager.Instance.hitPosition);

            Vector3 rot = transform.localEulerAngles;
            rot.x = 0;
            rot.z = 0;

            transform.localEulerAngles = rot;
        }

        public void ChangeShield(int shieldType)
        {
            switch ((ShieldType)shieldType)
            {
                case ShieldType.LITLLE:
                    _littleShield.SetActive(true);
                    _bigShield.SetActive(false);
                    break;
                case ShieldType.BIG:
                    _littleShield.SetActive(false);
                    _bigShield.SetActive(true);
                    break;
            }
        }

        public void ChangeShield(ShieldType shieldType)
        {
            switch (shieldType)
            {
                case ShieldType.LITLLE:
                    _littleShield.SetActive(true);
                    _bigShield.SetActive(false);
                    break;
                case ShieldType.BIG:
                    _littleShield.SetActive(false);
                    _bigShield.SetActive(true);
                    break;
            }
        }

        //private void OnTriggerEnter(Collider other)
        //{
        //    if(other.tag == "Ennemy")
        //    {
        //        onHit.Invoke();
        //        other.GetComponent<Mob>().scoreOnDeath = scoreDeathEnemy;
            
        //    }
        //}
    }
}
