using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatorController : MonoBehaviour
{
    public enum RotationDirection
    {
        UP, DOWN, LEFT, RIGHT
    }

    public RotationDirection direction;
}
