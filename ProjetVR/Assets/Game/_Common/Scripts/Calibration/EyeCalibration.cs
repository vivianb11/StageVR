using UnityEngine;
using UnityEngine.Events;

public class EyeCalibration : MonoBehaviour
{
    public Vector3[] eyeOffset;

    public GameObject cursor;

    private int calibrationPoints = 4;
    private int currentPoint = 0;

    public UnityEvent OnCalibrationDone;
    public UnityEvent Enabled;

    private void OnEnable()
    {
        if (!cursor)
            cursor = GameObject.FindGameObjectWithTag("Cursor");

        eyeOffset = new Vector3[calibrationPoints];

        Enabled?.Invoke();
    }

    public void Calibrate(Transform place)
    {
        currentPoint++;

        Vector3 directionToObject = place.position - Camera.main.transform.position;
        Vector3 eyeDirection = cursor.transform.position - Camera.main.transform.position;

        eyeOffset[currentPoint - 1] = directionToObject - eyeDirection;

        if (currentPoint >= calibrationPoints)
        {
            // calculate the average of the angles
            Vector3 sum = Vector3.zero;
            for (int i = 0; i < eyeOffset.Length; i++)
            {
                sum += eyeOffset[i];
            }
            EyeTrackingData.eyeOffset = sum / eyeOffset.Length;

            Debug.Log("Calibration done. Eye offset: " + EyeTrackingData.eyeOffset);

            OnCalibrationDone?.Invoke();
            SceneLoader.Instance.LodScene(1);
        }
    }

    public void ApplyCalibration()
    {
        // addes the eye rotation offset to the cursor
        cursor.transform.localEulerAngles += EyeTrackingData.eyeOffset;
    }
}
