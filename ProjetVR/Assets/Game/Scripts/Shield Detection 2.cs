using UnityEngine;

public class ShieldDetection2 : MonoBehaviour
{
    [SerializeField] private float Speed = 20f;
    [SerializeField] private Transform followPos = null; //Delete
    [SerializeField] private Material activeMat;
    [SerializeField] private Material inactiveMat;
    [SerializeField] private GameObject shieldColliderUp;

    private GameObject lastPanel;

    private void Update()
    {
        Quaternion fromRotation = this.transform.rotation;
        Vector3 fromRotation2 = this.transform.rotation.eulerAngles;
        Vector3 hitPosition = EyeManager.Instance.hitPosition;
        /*
        //Changement couleur panel
        GameObject Panel = EyeManager.Instance.hitCollider as GameObject;
        MeshRenderer panelRenderer = Panel.GetComponent<MeshRenderer>();
        MeshRenderer lastPanelRenderer = lastPanel.GetComponent<MeshRenderer>();

        
        //Hit new panel
        if (Panel.CompareTag("Panel") && lastPanel != Panel)
        {
            lastPanelRenderer.material = inactiveMat;
            panelRenderer.material = activeMat;
        }

        lastPanel = Panel;
        */

        /*
        //Target set to the followPos gameObject
        Quaternion rotTarget = Quaternion.LookRotation(followPos.position - this.transform.position, Vector3.back);
        this.transform.rotation = Quaternion.RotateTowards(fromRotation, rotTarget, Speed * Time.deltaTime);

        Debug.Log(followPos.position);
        */

        /*
        //Target set to the eye tracker using quaternions
        if (hitPosition.y > 0)
        {
            Quaternion rotTarget = Quaternion.LookRotation(hitPosition - this.transform.position, Vector3.back);
            this.transform.rotation = Quaternion.RotateTowards(fromRotation, rotTarget, Speed * Time.deltaTime);
        }
        else
        {
            Quaternion rotTarget = Quaternion.LookRotation(hitPosition - this.transform.position, Vector3.down);
            this.transform.rotation = Quaternion.RotateTowards(fromRotation, rotTarget, Speed * Time.deltaTime);
        }
        Debug.Log(hitPosition);
        */


        //Target set to the eye tracker using vector3
        /*
        Quaternion rotTarget2;

        if (hitPosition.y > 0)
        {
            Quaternion rotTarget = Quaternion.LookRotation(hitPosition - this.transform.position, Vector3.back);
            this.transform.rotation = Quaternion.RotateTowards(fromRotation, rotTarget, Speed * Time.deltaTime);
        }
        else
        {
            Quaternion rotTarget = Quaternion.LookRotation(hitPosition - this.transform.position, Vector3.down);
            this.transform.rotation = Quaternion.RotateTowards(fromRotation, rotTarget, Speed * Time.deltaTime);
            rotTarget2 = rotTarget.Euler;
            rotTarget2 = Quaternion.Euler(rotTarget2.x, 0, rotTarget2.z);
            this.transform.rotation = rotTarget2;
        }
        Debug.Log(hitPosition);
        */
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other == shieldColliderUp)
        {
            Debug.Log("UPUPUPUUPUPUPUPUPUPUPUPUPUPUPUPUPUPUP");
        }
    }
}