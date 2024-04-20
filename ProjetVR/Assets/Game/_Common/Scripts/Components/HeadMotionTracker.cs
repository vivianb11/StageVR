using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HeadMotionTracker : MonoBehaviour
{
    public enum PlayerExcitement
    {
        Excited,
        Normal,
        Calme
    }

    public static HeadMotionTracker Instance;

    private List<float> distances = new();

    private Vector3 lastPosition, secndLastPosition;

    private GameObject head;

    [Header("Tracker Parameters")]
    public float RecordingSample = 2f;

    public float ExcitedThreshold = 0.5f, NormalThreshold = 0.1f;

    [Space(10)]
    public UnityEvent Excited, Normal, Calme;

    private PlayerExcitement playerExcitement;

    public float GetTilt
    {
        get
        {
            Vector3 flatVector = transform.forward;
            flatVector.y = 0;

            return Vector3.Angle(flatVector, transform.forward);
        }
        private set { }
    }

    private void Awake()
    {
        if (Instance is null)
            Instance = this;
        else
            Destroy(gameObject);

        //add a empty gameobject as a child and reset its position to 0,0,1
        head = new GameObject("Tracker");
        head.transform.SetParent(transform);
        head.transform.localPosition = new Vector3(0, 0, 1);

        for (int i = 0; i < RecordingSample / Time.fixedDeltaTime; i++)
        {
            distances.Add(0);
        }

        playerExcitement = PlayerExcitement.Calme;
    }

    private void Update()
    {
        float TotalDistance = GetDistance();

        if (TotalDistance > ExcitedThreshold)
        {
            if (playerExcitement != PlayerExcitement.Excited)
            {
                playerExcitement = PlayerExcitement.Excited;
                Excited.Invoke();
            }
        }
        else if (TotalDistance > NormalThreshold)
        {
            if (playerExcitement != PlayerExcitement.Normal)
            {
                playerExcitement = PlayerExcitement.Normal;
                Normal.Invoke();
            }
        }
        else
        {
            if (playerExcitement != PlayerExcitement.Calme)
            {
                playerExcitement = PlayerExcitement.Calme;
                Calme.Invoke();
            }
        }

        Debug.Log(GetTilt);
    }

    private void FixedUpdate()
    {
        secndLastPosition = lastPosition;
        lastPosition = head.transform.position;

        distances.Add(Vector3.Distance(lastPosition, secndLastPosition));

        while (distances.Count > RecordingSample/Time.fixedDeltaTime)
        {
            distances.RemoveAt(0);
        }
    }

    public float GetDistance()
    {
        float distance = 0;

        foreach (var d in distances)
        {
            distance += d;
        }

        return distance;
    }
}
