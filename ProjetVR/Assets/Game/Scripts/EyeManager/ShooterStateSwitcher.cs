using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterStateSwitcher : MonoBehaviour
{
    public Shooter shooter;
    public Shooter.ShootType shootType;

    public void SwitchState()
    {
        shooter.SwitchShootType(shootType);
    }
}
