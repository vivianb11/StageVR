using UnityEngine;
using UnityEngine.Events;

public class Shield : MonoBehaviour
{
    public GameObject targetObject;
    public float moveSpeed = 5f;
    public float returnSpeed = 2f;

    public bool isColliding = false;
    private bool isTweening = false;
    public float moveDuration = 0.5f;

    public UnityEvent OnShieldHit;

    private Tween tweener;
    private ProtectedToothBehaviours tooth;


    private void Awake()
    {
        tweener = GetComponent<Tween>();
        tooth = targetObject.GetComponent<ProtectedToothBehaviours>();
    }

    
    //l'idée a joss et thomas a renomé la fonction (le gros du travail)
    public void TweenerChecker(bool tweening) => isTweening = tweening;
    


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Ennemy"))
        {
            isColliding = true;
            tooth.enemyPoints = other.GetComponent<Mob>().scoreOnDeath;
            OnShieldHit.Invoke();

            if (!isTweening)
            {
                tweener.tweenMontages[0].tweenProperties[1].to = gameObject.transform.localPosition;
                tweener.tweenMontages[0].tweenProperties[0].to = new Vector3(gameObject.transform.localPosition.x / 0.75f, gameObject.transform.localPosition.y / 0.75f, gameObject.transform.localPosition.z / 0.75f);
                tweener.PlayMontages();
            }
            
        }
    }

    
}
