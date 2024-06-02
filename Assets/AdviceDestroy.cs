using UnityEngine;
using UnityEngine.Events;

public class AdviceDestroy : MonoBehaviour
{
    public float moveSpeed = 5f; // Adjust this to control the speed of movement
    public float destroyDelay = 4f; // Time before destroying the object
    public float moveDelay = 7f;

    private bool isMoving = false;
    private float elapsedTime = 0f;
    public UnityEvent OnSlide = new();

    void Start()
    {
        if (GameManager.Instance.currentReloadCause == GameManager.ReloadCause.DEATH)
        {
            OnSlide.Invoke();
            gameObject.SetActive(false);
            print(GameManager.Instance.currentReloadCause);
        }
        else
        {
            // Start moving after 4 seconds
            Invoke("StartMoving", moveDelay);
        }
    }

    void Update()
    {
        if (isMoving)
        {
            // Move the object to the left
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);

            // Check if the object should be destroyed
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= destroyDelay)
            {
                OnSlide.Invoke();
                Destroy(gameObject);
            }
        }
    }

    void StartMoving()
    {
        isMoving = true;
    }
}
