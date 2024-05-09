using UnityEngine;
using UnityEngine.UI;

public class AutoFocus : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Selectable>().Select();
    }
}
