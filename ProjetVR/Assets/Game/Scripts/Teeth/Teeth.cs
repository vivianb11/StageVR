using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teeth : MonoBehaviour
{
    public enum TeethState
    {
        WHITE, YELLOW, BLACK
    }

    public TeethState state;

    [SerializeField]
    private MeshRenderer mesh;

    [SerializeField]
    private float dirtyDelay;

    private void Start()
    {
        SetTeethColor();
        StartCoroutine(DirtyTimer(dirtyDelay));
    }

    public void SetTeethColor()
    {
        switch (state)
        {
            case TeethState.WHITE:
                mesh.material.color = Color.white;
                break;
            case TeethState.YELLOW:
                mesh.material.color = Color.yellow;
                break;
            case TeethState.BLACK:
                mesh.material.color = Color.black;
                break;
        }
    }

    public void CleanTeeth()
    {
        state = TeethState.WHITE;
        SetTeethColor();

        StopAllCoroutines();
        StartCoroutine(DirtyTimer(dirtyDelay));
    }

    private IEnumerator DirtyTimer(float delay)
    {
        while (state != TeethState.BLACK)
        {
            yield return new WaitForSeconds(delay);

            state = state + 1;
            SetTeethColor();
        }
    }
}
