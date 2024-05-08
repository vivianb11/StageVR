using UnityEngine;

public class PlayerInstance : MonoBehaviour
{
    [HideInInspector]
    public static PlayerInstance Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            foreach (MonoBehaviour component in gameObject.GetComponents<MonoBehaviour>())
            {
                component.enabled = true;
            }

            transform.GetChild(0).gameObject.SetActive(true);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
}
