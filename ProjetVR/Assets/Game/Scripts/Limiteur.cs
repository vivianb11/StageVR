using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;
using NaughtyAttributes;

public class Limiteur : MonoBehaviour
{
    private enum LimiteurMode
    {
        Self,
        other,
        others
    }

    [Header("Visual")]
    public bool showVisual = true;

    EyeManager eyeRaycaster;

    private Interactable interacable;
    private List<Interactable> interacables;

    private Vector3 center;

    [Header("Limiteur Params")]
    [SerializeField]
    private LimiteurMode _limiteurMode;

    [ShowIf("_limiteurMode", LimiteurMode.other)]
    public GameObject other;

    [ShowIf("_limiteurMode", LimiteurMode.others)]
    public List<GameObject> others;

    [Space(10)]
    public float maxDistance = 1f;
    public float errorBuffer = 0.1f;

    private void Awake()
    {
        switch (_limiteurMode)
        {
            case LimiteurMode.Self:
                TryGetComponent<Interactable>(out Interactable interacable);
                break;

            case LimiteurMode.other:
                if (other.TryGetComponent<Interactable>(out Interactable inter2))
                    interacable = inter2;
                break;

            case LimiteurMode.others:
                foreach (GameObject go in others)
                {
                    if (go.TryGetComponent<Interactable>(out Interactable inter3))
                        interacables.Add(inter3);
                    else
                        interacables.Add(null);
                }
                break;

            default:
                break;
        }

    }

    private void Start()
    {
        eyeRaycaster = EyeManager.Instance;

        center = transform.position;
    }

    private void FixedUpdate()
    {
        if (_limiteurMode == LimiteurMode.Self)
        {
            ClampTransform(gameObject);
        }
        else if (_limiteurMode == LimiteurMode.other)
        {
            ClampTransform(other);
        }
        else if (_limiteurMode == LimiteurMode.others)
        {
            foreach (GameObject go in others)
            {
                ClampTransform(go);
            }
        }

        if (eyeRaycaster.GetGrabbedBody())
        {
            if (Vector3.Distance(eyeRaycaster.GetGrabbedBodyDestination(),center) > maxDistance + errorBuffer)
            {
                Desinteractor(eyeRaycaster.GetGrabbedBody().gameObject);
            }
        }
    }

    private void ClampTransform(GameObject gameObject)
    {
        if (Vector3.Distance(center, gameObject.transform.position) > maxDistance)
        {
            gameObject.transform.position = center + (gameObject.transform.position - center).normalized * maxDistance;
        }
    }

    private void Desinteractor(GameObject gameObject)
    {
        switch (_limiteurMode)
        {
            case LimiteurMode.Self:
                interacable.DeSelect();
                break;

            case LimiteurMode.other:
                if (gameObject == other)
                    interacable.DeSelect();
                break;

            case LimiteurMode.others:
                for (int i = 0; i < others.Count; i++)
                {
                    if (gameObject == others[i])
                        interacables[i].DeSelect();
                }
                break;

            default:
                break;
        }
    }

    private void OnDrawGizmos()
    {
        if (showVisual)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(this.transform.position, maxDistance);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(this.transform.position, maxDistance + errorBuffer);
        }
    }
}
