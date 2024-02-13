using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public bool snaping = true;

    [SerializeField]
    private int snapValue = 90;

    public void UpRotate()
    {
        transform.Rotate(Vector3.up, snapValue);
    }

    public void DownRotate()
    {
        transform.Rotate(Vector3.down, snapValue);
    }

    public void LeftRotate()
    {
        transform.Rotate(Vector3.left, snapValue);
    }

    public void RightRotate()
    {
        transform.Rotate(Vector3.right, snapValue);
    }
}
