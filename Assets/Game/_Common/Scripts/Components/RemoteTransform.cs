using NaughtyAttributes;
using UnityEngine;

[ExecuteInEditMode]
public class RemoteTransform : MonoBehaviour
{
    public Transform target;

    [Foldout("Parameters")]
    public bool syncPosition = true;
    [ShowIf("syncPosition")]
    [Foldout("Parameters")]
    public bool lerpPosition = false;
    [ShowIf("lerpPosition")]
    [Foldout("Parameters")]
    public float positionLerpSpeed = 1f;

    [Space(10)]

    [Foldout("Parameters")]
    public bool syncRotation = true;
    [ShowIf("syncRotation")]
    [Foldout("Parameters")]
    public bool lerpRotation = false;
    [ShowIf("lerpRotation")]
    [Foldout("Parameters")]
    public float rotationLerpSpeed = 1f;
    [Foldout("Parameters")]
    [ShowIf("syncRotation")]
    public bool syncRotationX = true;
    [Foldout("Parameters")]
    [ShowIf("syncRotation")]
    public bool syncRotationY = true;
    [Foldout("Parameters")]
    [ShowIf("syncRotation")]
    public bool syncRotationZ = true;

    [Space(10)]

    [Foldout("Parameters")]
    public bool syncScale = true;
    [ShowIf("syncScale")]
    [Foldout("Parameters")]
    public bool lerpScale = false;
    [ShowIf("lerpScale")]
    [Foldout("Parameters")]
    public float scaleLerpSpeed = 1f;

    private void Update()
    {
        if (target == null)
            return;

        if (syncPosition)
        {
            if (lerpPosition)
                target.position = Vector3.Lerp(target.position, transform.position, positionLerpSpeed * Time.deltaTime);
            else
                target.position = transform.position;
        }

        if (syncRotation)
        {
            Vector3 targetRot = target.eulerAngles;
            if (syncRotationX)
                targetRot.x = transform.rotation.eulerAngles.x;
            if (syncRotationY)
                targetRot.y = transform.rotation.eulerAngles.y;
            if (syncRotationZ)
                targetRot.z = transform.rotation.eulerAngles.z;

            if (lerpRotation)
                target.rotation = Quaternion.Lerp(target.rotation, Quaternion.Euler(targetRot), positionLerpSpeed * Time.deltaTime);
            else
                target.rotation = Quaternion.Euler(targetRot);
        }

        if (syncScale)
        {
            if (lerpScale)
                target.position = Vector3.Lerp(target.localScale, transform.localScale, positionLerpSpeed * Time.deltaTime);
            else
                target.localScale = transform.parent.localScale.magnitude * transform.localScale;
        }
    }
}
