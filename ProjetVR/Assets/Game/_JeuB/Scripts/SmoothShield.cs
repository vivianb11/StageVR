using UnityEngine;
using UnityEngine.Events;


namespace JeuB
{
    public class SmoothShield : MonoBehaviour
    {
        //[SerializeField] UnityEvent onHit = new UnityEvent();
        //private int scoreDeathEnemy;

        void Update()
        {
            transform.LookAt(EyeManager.Instance.hitPosition);

            Vector3 rot = transform.localEulerAngles;
            rot.x = 0;
            rot.z = 0;

            transform.localEulerAngles = rot;
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
