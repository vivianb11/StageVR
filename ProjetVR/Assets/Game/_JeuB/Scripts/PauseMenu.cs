using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject _panel;

    private void Update()
    {
        if (OVRInput.Get(OVRInput.RawButton.B))
            _panel.SetActive(!_panel.activeInHierarchy);
    }
}
