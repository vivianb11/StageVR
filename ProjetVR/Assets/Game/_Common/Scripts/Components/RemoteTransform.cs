using NaughtyAttributes;
using UnityEngine;

[ExecuteInEditMode]
public class RemoteTransform : MonoBehaviour
{
    public Transform target;

    [Foldout("Parameters")]
    public bool syncPosition = true;
    [Foldout("Parameters")]
    public bool syncRotation = true;
    [Foldout("Parameters")]
    public bool syncScale = true;

    private void Update()
    {
        if (target == null)
            return;

        if (syncPosition)
            target.position = transform.position;

        if (syncRotation)
            target.rotation = transform.rotation;

        if (syncScale)
            target.localScale = transform.parent.localScale.magnitude * transform.localScale;
    }
}
