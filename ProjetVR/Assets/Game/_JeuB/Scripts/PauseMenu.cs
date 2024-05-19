using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject _panel;

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.B))
        {
            _panel.SetActive(!_panel.activeInHierarchy);
            GameManager.Instance.gamePaused = _panel.activeInHierarchy;
        }
    }
}
