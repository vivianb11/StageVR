using UnityEngine;

public class Editor_Controleur : MonoBehaviour
{
#if UNITY_EDITOR

    public bool active = true;

    private void Awake()
    {
        Cursor.visible = !active;
        Cursor.lockState = !active ? CursorLockMode.None : CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (!active)
            return;

        if (Input.GetKeyUp(KeyCode.LeftControl))
            HideCursor();
        else if (Input.GetKeyDown(KeyCode.LeftControl))
            ShowCursor();

        if (Input.GetKey(KeyCode.LeftControl))
            CursorControl();
    }

    public void CursorControl()
    {
        Vector3 mousePos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        Vector3 target = ray.GetPoint(10);

        this.transform.LookAt(target);
    }

    private void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    private void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
#endif

}
