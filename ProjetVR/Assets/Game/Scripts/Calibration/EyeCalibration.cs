using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EyeCalibration : MonoBehaviour
{
    public Vector3[] eyeOffset;

    public GameObject cursor;

    private int calibrationPoints = 4;
    private int currentPoint = 0;

    public UnityEvent OnCalibrationDone;

    private void Start()
    {
        cursor = GameObject.FindGameObjectWithTag("Cursor");

        eyeOffset = new Vector3[calibrationPoints];
    }

    public void Calibrate(Transform place)
    {
        currentPoint++;

        // get the angle between the camera and the cursor and compare it to the angle between the camera and the calibration point
        Vector3 cursorPos = cursor.transform.position;
        Vector3 cameraPos = Camera.main.transform.position;

        Vector3 cursorDir = cursorPos - cameraPos;
        Vector3 placeDir = place.position - cameraPos;

        // gets the vector3 angle to add to the cursor so it would be pointing to the calibration point
        Vector3 angle = Vector3.Cross(cursorDir, placeDir);

        // get the difference between the two angles
        eyeOffset[currentPoint - 1] = angle;

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
        }
    }

    public void ApplyCalibration()
    {
        // addes the eye rotation offset to the cursor
        cursor.transform.localEulerAngles += EyeTrackingData.eyeOffset;
    }
}
