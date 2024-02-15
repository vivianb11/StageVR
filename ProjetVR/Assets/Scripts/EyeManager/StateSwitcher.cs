using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateSwitcher : MonoBehaviour
{
    public EyeManager.ManagerState managerState;

    public void SetManagerState()
    {
        EyeManager.Instance.SwitchState(managerState);
    }
}