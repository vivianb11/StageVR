using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject _panel;

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.B))
            _panel.SetActive(!_panel.activeInHierarchy);
    }
}
