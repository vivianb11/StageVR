using NaughtyAttributes;
using UnityEngine;

public class ShieldDetection : MonoBehaviour
{
    [SerializeField] [ReadOnly] private GameObject currentActiveShield;

    [SerializeField] float minDetectionRange = 1.0f;

    [SerializeField] Transform[] points;

    /*
    [SerializeField] private Material activeMat;
    [SerializeField] private Material inactiveMat;
    private GameObject Panel;
    private GameObject lastPanel;
    */


    void Start()
    {
        currentActiveShield = null;
        /*
        Panel = EyeManager.Instance.hitCollider as GameObject;
        lastPanel = EyeManager.Instance.hitCollider as GameObject;
        */
    }

    private void FixedUpdate()
    {
        /*
        if (EyeManager.Instance != null || EyeManager.Instance.hitCollider != null)
        {
            ChangePanel();
        }
        */

        Vector3 hitPosition = EyeManager.Instance.hitPosition;
        float distanceToHitpoint = Vector3.Distance(transform.position, hitPosition);

        if (distanceToHitpoint < minDetectionRange)
            return;

        Transform closestPoint = points[0];
        float closestDistance = Vector3.Distance(closestPoint.position, hitPosition);

        foreach (Transform t in points)
        {
            float distance = Vector3.Distance(t.position, hitPosition);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPoint = t;
            }
        }
        Activate(closestPoint.parent);
    }

    public void changeDirection(GameObject obj)
    {
        DeactivateCurrentShield();

        obj.SetActive(true);

        currentActiveShield = obj;
    }


    public void Activate(GameObject obj)
    {
        DeactivateCurrentShield();

        obj.SetActive(true);

        currentActiveShield = obj;
    }

    public void Activate(Transform obj)
    {
        DeactivateCurrentShield();

        obj.gameObject.SetActive(true);

        currentActiveShield = obj.gameObject;
    }

    private void DeactivateCurrentShield()
    {
        if (currentActiveShield != null && currentActiveShield.activeSelf)
        {
            currentActiveShield.SetActive(false);
        }
    }

    /*
    public void ChangePanel()
    {
        //Changement couleur panel
        Panel = EyeManager.Instance.hitCollider as GameObject;
        MeshRenderer panelRenderer = Panel.GetComponent<MeshRenderer>();
        MeshRenderer lastPanelRenderer = lastPanel.GetComponent<MeshRenderer>();


        //Hit new panel
        if (Panel.CompareTag("Panel") && lastPanel != Panel)
        {
            lastPanelRenderer.material = inactiveMat;
            panelRenderer.material = activeMat;
        }

        lastPanel = Panel;
    }
    */
}