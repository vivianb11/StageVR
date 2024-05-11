using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Navigatable : MonoBehaviour
{
    public static Navigatable SelectedItem;

    public enum Direction { LEFT, RIGHT, UP, DOWN }

    public bool autoSelect;

    [Space(10)]

    public SpriteRenderer graphicTarget;

    public Sprite selectedSprite;
    public Sprite deselectedSprite;

    [Space(10)]

    public Navigatable up;
    public Navigatable down;
    public Navigatable left;
    public Navigatable right;

    [Header("Events")]
    public UnityEvent Pressed;

    private static bool canMove = true;

    private void Start()
    {
        Deselect();
        if (autoSelect)
        {
            if (SelectedItem != null)
            {
                Debug.LogWarning("An item is already selected");
                return;
            }
            Select();
        }
    }

    private void Update()
    {
        if (SelectedItem == this)
            HandleInputs();
    }

    public void Select()
    {
        SelectedItem = this;
        graphicTarget.sprite = selectedSprite;
    }

    public void Deselect()
    {
        if (SelectedItem == this)
            SelectedItem = null;
        graphicTarget.sprite = deselectedSprite;
    }

    public void MoveTo(Direction moveDirection)
    {
        switch (moveDirection)
        {
            case Direction.LEFT:
                SwitchItem(left);
                break;
            case Direction.RIGHT:
                SwitchItem(right);
                break;
            case Direction.UP:
                SwitchItem(up);
                break;
            case Direction.DOWN:
                SwitchItem(down);
                break;
        }

        canMove = false;

        StartCoroutine(MoveDelay());
    }

    private void SwitchItem(Navigatable switchTarget)
    {
        if (switchTarget is null)
            return;

        Deselect();
        switchTarget.Select();
    }

    private void HandleInputs()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || OVRInput.GetDown(OVRInput.Button.One))
            Pressed?.Invoke();

        if (!canMove)
            return;

        if (Input.GetKey(KeyCode.LeftArrow) || OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).x == -.9f)
            MoveTo(Direction.LEFT);
        else if (Input.GetKey(KeyCode.RightArrow) || OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).x == .9f)
            MoveTo(Direction.RIGHT);
        else if(Input.GetKey(KeyCode.UpArrow) || OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).y == .9f)
            MoveTo(Direction.UP);
        else if(Input.GetKey(KeyCode.DownArrow) || OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).y == -.9f)
            MoveTo(Direction.DOWN);
    }

    private IEnumerator MoveDelay()
    {
        yield return new WaitForSeconds(0.5f);

        canMove = true;
    }
}
