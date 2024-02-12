using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadMotionTracker : MonoBehaviour
{
    private List<float> distances;

    private Vector3 lastPosition, secndLastPosition;

    private GameObject head;

    public float recordedTime = 2f;

    private void Awake()
    {
        //add a empty gameobject as a child and reset its position to 0,0,1
        head = new GameObject("Tracker");
        head.transform.SetParent(transform);
        head.transform.localPosition = new Vector3(0, 0, 1);
    }

    private void FixedUpdate()
    {
        secndLastPosition = lastPosition;
        lastPosition = head.transform.position;

        distances.Add(Vector3.Distance(lastPosition, secndLastPosition));

        if (distances.Count > recordedTime/Time.fixedDeltaTime)
        {
            distances.RemoveAt(0);
        }
    }

    public float GetDistance(float time)
    {
        if (time > recordedTime)
            return -1;

        float distance = 0;

        for (int i = 0; i < time/Time.fixedDeltaTime; i++)
        {
            distance += distances[i];
        }

        return distance;
    }
}
