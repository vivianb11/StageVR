using UnityEngine;

public class ToothPasteProjectile : Projectile
{
    private void OnTriggerEnter(Collider other)
    {
        if (!canCollid)
            return;

        if (other.gameObject.tag != "Cell")
        {
            gameObject.SetActive(false);
            return;
        }

        if (other.TryGetComponent(out CellBehavior cell) && cell.teethState == TeethState.Tartar && !cell.ToothPasteFull())
        {
            body.velocity = Vector3.zero;
            GetComponent<Collider>().enabled = false;

            StopAllCoroutines();
            body.isKinematic = true;
            transform.parent = other.transform;

            cell.IncreaseToothPasteAmount();

            canCollid = true;

            return;
        }

        gameObject.SetActive(false);
    }
}
