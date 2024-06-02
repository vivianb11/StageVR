using UnityEngine;

[ExecuteInEditMode]
public class TextScaleSyncronyser : MonoBehaviour
{
    public TextMesh tMesh;

    public GameObject target;

    public float widthOffset = 5f;
    public float heightOffset = 10f;

    private GameObject oldTarget;
    private Vector3 dS;

    Vector3 newScale;
    float charWidth;
    float charHeight;

    public void Update()
    {
        if (target == null)
        {
            if (oldTarget != null)
                oldTarget.transform.localScale = dS;

            oldTarget = null;

            return;
        }

        if (target != oldTarget)
        {
            if (oldTarget != null)
                oldTarget.transform.localScale = dS;

            oldTarget = target;

            charWidth = (tMesh.characterSize / tMesh.fontSize) * widthOffset;
            dS = target.transform.localScale;
        }

        charWidth = (tMesh.characterSize / tMesh.fontSize) * widthOffset;
        charHeight = (tMesh.characterSize / tMesh.fontSize) * heightOffset;

        newScale.x = (tMesh.GetLongestLine()).Length * charWidth + dS.x;
        newScale.y = tMesh.GetNumberOfLines() * charHeight + dS.y;
        newScale.z = dS.z;

        target.transform.localScale = Vector3.Lerp(target.transform.localScale, newScale, 0.1f);
    }
}
