using UnityEngine;

public class ChangeCrosshair : MonoBehaviour
{
    private GameObject cross;

    // Start is called before the first frame update
    void Start()
    {
        cross = GameObject.FindGameObjectWithTag("Cursor");

        cross.GetComponent<CrosshairLerp>().lerpSpeed = 0.5f;
    }
}
