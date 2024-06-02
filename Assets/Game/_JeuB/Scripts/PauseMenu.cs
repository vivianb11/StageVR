using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject _panel;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B) || OVRInput.GetDown(OVRInput.RawButton.B))
        {
            _panel.SetActive(!_panel.activeInHierarchy);
            GameManager.Instance.gamePaused = _panel.activeInHierarchy;
        }
    }

    public void Resume()
    {
        _panel.SetActive(false);
        GameManager.Instance.gamePaused = false;
    }
}
