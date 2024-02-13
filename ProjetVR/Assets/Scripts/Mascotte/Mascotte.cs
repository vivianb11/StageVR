using UnityEngine;
using System.Linq;

public class Mascotte : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer mesh;

    [SerializeField]
    private Color[] colors;

    private int index;

    private Teeth[] teeths;

    private Vector3 targetPosition;

    private void Start()
    {
        teeths = FindObjectsOfType<Teeth>();
        targetPosition = transform.position;
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime);
    }

    public void ChangeColor()
    {
        index++;
        if (index == colors.Length)
            index = 0;

        mesh.material.color = colors[index];
    }

    private Teeth GetClosestTeeth()
    {
        Teeth closest = null;

        Teeth[] dirtyTeeth = teeths.Where(item => item.state != Teeth.TeethState.GREEN).ToArray();

        foreach (Teeth t in dirtyTeeth)
        {
            if (closest == null)
            {
                closest = t;
                continue;
            }

            if (((int)closest.state) > ((int)closest.state))
            {
                closest = t;
                continue;
            }

            float closestTeethDistance = Vector3.Distance(closest.transform.position, transform.position);
            float currentTeethDistance = Vector3.Distance(t.transform.position, transform.position);

            if (currentTeethDistance < closestTeethDistance)
                closest = t;
        }

        return closest;
    }

    public void SetTask()
    {
        if (GetClosestTeeth())
            targetPosition = GetClosestTeeth().transform.position;
    }
}
