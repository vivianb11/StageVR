using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class BumperCar : MonoBehaviour
{
    public GameObject targetObject;
    public float moveSpeed = 5f;
    public float returnSpeed = 2f;

    public bool isColliding = false;
    public float moveDuration = 0.5f;

    public UnityEvent OnShieldHit;

    private Tween tweener;
    private ProtectedToothBehaviours tooth;

    private void Awake()
    {
        tweener = GetComponent<Tween>();
        tooth = targetObject.GetComponent<ProtectedToothBehaviours>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Ennemy"))
        {
            isColliding = true;
            tooth.enemyPoints = other.GetComponent<Mob>().scoreOnDeath;
            OnShieldHit.Invoke();
            //tweener.PlayMontages();
        }
    }
}
