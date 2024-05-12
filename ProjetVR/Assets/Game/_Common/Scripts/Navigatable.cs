using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Navigatable : MonoBehaviour
{
    public static Navigatable SelectedItem;

    [SerializeField] enum NavigationMode { MANUAL, VERTICAL, HORIZONTAL}

    [SerializeField] NavigationMode navigationMode;

    public enum Direction { LEFT, RIGHT, UP, DOWN }

    public bool autoSelect;

    [Space(10)]

    public SpriteRenderer graphicTarget;

    public Sprite selectedSprite;
    public Sprite deselectedSprite;

    [Space(10)]

    [ShowIf("navigationMode", NavigationMode.MANUAL)]
    public Navigatable up;
    [ShowIf("navigationMode", NavigationMode.MANUAL)]
    public Navigatable down;
    [ShowIf("navigationMode", NavigationMode.MANUAL)]
    public Navigatable left;
    [ShowIf("navigationMode", NavigationMode.MANUAL)]
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
                Debug.LogWarning("An item is already selected");
            else
                Select();
        }

        SetupDirection();
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

    private void SetupDirection()
    {
        switch (navigationMode)
        {
            case NavigationMode.MANUAL:
                break;
            case NavigationMode.VERTICAL:
                for (int i = 0; i < transform.parent.childCount; i++)
                {
                    if (transform.parent.GetChild(i) == transform)
                    {
                        if (i - 1 >= 0) up = transform.parent.GetChild(i - 1).GetComponent<Navigatable>();
                        if (i + 1 < transform.parent.childCount) down = transform.parent.GetChild(i + 1).GetComponent<Navigatable>();
                    }
                }
                break;
            case NavigationMode.HORIZONTAL:
                for (int i = 0; i < transform.parent.childCount; i++)
                {
                    if (transform.parent.GetChild(i) == transform)
                    {
                        if (i - 1 >= 0) left = transform.parent.GetChild(i - 1).GetComponent<Navigatable>();
                        if (i + 1 < transform.parent.childCount) right = transform.parent.GetChild(i + 1).GetComponent<Navigatable>();
                    }
                }
                break;
        }
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
