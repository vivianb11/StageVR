using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BainDeBoucheProjectile : Projectile
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out CellBehavior cell))
        {
            cell.toothManager.RemoveSmellAmount();
        }
    }
}
