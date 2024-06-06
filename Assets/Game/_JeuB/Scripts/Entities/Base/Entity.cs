using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace JeuB
{
    public abstract class Entity : MonoBehaviour, IDamageable
    {
        public int MaxHealth = 1;
        public int Health { get; private set; }

        public Transform target;
        public float moveSpeed = 1;

        [Foldout("Events")]
        public UnityEvent OnDamaged, OnHeal, OnDeath;

        private void Start()
        {
            Health = MaxHealth;

            EntityStart();
        }

        private void Update()
        {
            if (GameManager.Instance.gamePaused)
                return;

            Move();

            EntityUpdate();
        }

        private void FixedUpdate()
        {
            EntityFixedUpdate();
        }

        protected abstract void EntityStart();

        protected abstract void EntityUpdate();

        protected virtual void EntityFixedUpdate() { }

        public abstract void Kill();

        public virtual void Move()
        {

            transform.LookAt(target.position, transform.parent.up);

            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime, Space.Self);
        }

        public void Heal(int healAmount)
        {
            Health = Mathf.Clamp(Health + healAmount, 0, MaxHealth);

            OnHeal?.Invoke();
        }

        public void ReceiveDamage(int damage)
        {
            Health = Mathf.Clamp(Health - damage, 0, MaxHealth);

            OnDamaged?.Invoke();

            if (Health > 0) return;

            Kill();
            OnDeath?.Invoke();
        }
    }
}