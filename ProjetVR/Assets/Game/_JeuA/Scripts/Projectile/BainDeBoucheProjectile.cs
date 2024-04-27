using UnityEngine;

public class BainDeBoucheProjectile : Projectile
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out CellBehavior cell))
        {
            Debug.Log("Collid with cell");
            cell.toothManager.MinusSmell();
        }
    }
}
