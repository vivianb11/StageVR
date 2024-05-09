using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HeadMotionTracker : MonoBehaviour
{
    public enum HeadStates
    {
        Excited,
        Normal,
        Calme
    }

    public static HeadMotionTracker Instance;

    private List<float> distances = new();

    private Vector3 lastPosition, secndLastPosition;

    [Header("Tracker Parameters")]
    public float RecordingSample = 2f;

    public float ExcitedThreshold = 0.5f, NormalThreshold = 0.1f;

    [Space(10)]
    public UnityEvent Excited, Normal, Calme;

    private HeadStates headState;

    private Transform forward;

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
        
        forward = new GameObject("ForwardTracker").transform;
        forward.SetParent(this.transform);

        for (int i = 0; i < RecordingSample / Time.fixedDeltaTime; i++)
        {
            distances.Add(0);
        }

        headState = HeadStates.Calme;
    }

    private void FixedUpdate()
    {
        secndLastPosition = lastPosition;
        lastPosition = forward.transform.position;

        distances.Add(Vector3.Distance(lastPosition, secndLastPosition));

        while (distances.Count > RecordingSample/Time.fixedDeltaTime)
        {
            distances.RemoveAt(0);
        }

        float TotalDistance = GetDistance();

        if (TotalDistance > ExcitedThreshold)
        {
            SwitchState(HeadStates.Excited);
        }
        else if (TotalDistance > NormalThreshold)
        {
            SwitchState(HeadStates.Normal);
        }
        else
        {
            SwitchState(HeadStates.Calme);
        }
    }

    private float GetDistance()
    {
        float distance = 0;

        foreach (var d in distances)
        {
            distance += d;
        }

        return distance;
    }

    private void SwitchState(HeadStates newState)
    {
        if (newState == headState)
            return;

        headState = newState;

        switch (newState)
        {
            case HeadStates.Excited:
                Excited?.Invoke();
                break;
            case HeadStates.Normal:
                Normal?.Invoke();
                break;
            case HeadStates.Calme:
                Calme?.Invoke();
                break;
        }

        Debug.Log(headState.ToString());
    }
}
