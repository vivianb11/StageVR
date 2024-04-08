using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverOverlay : MonoBehaviour
{
    [SerializeField] float apparitionDelay;

    public void DisplayScoreTab() => Invoke("Show", apparitionDelay);
    private void Show() => gameObject.SetActive(true);
}
