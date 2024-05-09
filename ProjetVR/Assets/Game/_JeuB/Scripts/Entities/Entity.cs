using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

namespace JeuB
{
    public abstract class Entity : MonoBehaviour, IDamageable
    {
        public int MaxHealth;
        public int health;

        public Transform target;
        public float moveSpeed;

        [Foldout("Events")]
        public UnityEvent OnDamaged, OnHeal, OnDeath;

        private void Start()
        {
            health = MaxHealth;

            EntityStart();
        }

        private void Update()
        {
            EntityUpdate();
        }

        protected abstract void EntityStart();

        protected abstract void EntityUpdate();

        public abstract void Kill();

        public virtual void Move()
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, target.position, moveSpeed * Time.deltaTime);
        }

        public void Heal(int healAmount)
        {
            health = Mathf.Clamp(health + healAmount, 0, MaxHealth);

            OnHeal?.Invoke();
        }

        public void ReceiveDamage(int damage)
        {
            health = Mathf.Clamp(health - damage, 0, MaxHealth);

            OnDamaged?.Invoke();

            if (health <= 0)
            {
                Kill();
                OnDeath?.Invoke();
            }
        }
    }
}