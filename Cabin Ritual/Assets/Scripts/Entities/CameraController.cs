using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public enum ERotationAxis
    {
        XOnly,
        YOnly,
        Both,
        Locked
    }

    public enum ECamRotationAxis
    {
        X,
        Y
    }


    // A reference to the camera (allows axis controlling without moving the character's body).
    public Transform Cam;

    // The axis the camera will control.
    public ECamRotationAxis CamRotationAxis = ECamRotationAxis.Y;

    // The axis the mouse will effect.
    public ERotationAxis RotationAxis = ERotationAxis.Both;

    // The sensitivity for the X axis.
    public float XSensitivity = 1.0f;

    // The sensitivity for the Y axis.
    public float YSensitivity = 1.0f;

    // The maximum angle the user can look up at.
    public float LookUpCap = 45.0f;

    // The minimum angle the user can look down at.
    public float LookDownCap = -45.0f;



    // Determines how much to rotate along the X axis (based off mouse movement speed).
    private float XStrength;

    // Determines how much to rotate along teh Y axis (based off the mouse movement speed).
    private float YStrength;



    // Controls the body and camera (if any) for the rotations assuming the character has 
    void FixedUpdate()
    {
        switch (RotationAxis)
        {
            case ERotationAxis.XOnly:
                // Get the current rotation strength.
                XStrength = Input.GetAxis("Mouse X") * XSensitivity;

                if (Cam)
                {
                    switch (CamRotationAxis)
                    {
                        // If the user has the X axis specified for the camera.
                        case ECamRotationAxis.X:
                            Cam.localRotation = Quaternion.Euler(new Vector3(0.0f, XStrength, 0.0f) + Cam.localRotation.eulerAngles);
                            break;

                        // If the user has the Y axis specified for the camera.
                        case ECamRotationAxis.Y:
                            transform.localRotation = Quaternion.Euler(new Vector3(0.0f, XStrength, 0.0f) + transform.localRotation.eulerAngles);
                            break;
                    }
                }
                else
                {
                    // If the user does not have a camera attached.
                    transform.localRotation = Quaternion.Euler(new Vector3(0.0f, XStrength, 0.0f) + transform.localRotation.eulerAngles);
                }
                break;


            case ERotationAxis.YOnly:
                // Get the current rotation strength.
                YStrength = Input.GetAxis("Mouse Y") * YSensitivity;
                if (Cam)
                {
                    switch (CamRotationAxis)
                    {
                        // If the user has the X axis specified for the camera.
                        case ECamRotationAxis.X:
                            transform.localRotation = Quaternion.Euler(new Vector3(-YStrength, 0.0f, 0.0f) + transform.localRotation.eulerAngles);
                            break;

                        // If the user has the Y axis specified for the camera.
                        case ECamRotationAxis.Y:
                            Cam.localRotation = Quaternion.Euler(new Vector3(-YStrength, 0.0f, 0.0f) + Cam.localRotation.eulerAngles);
                            break;

                    }
                }
                else
                {
                    // If the user does not have a camera attached.
                    transform.localRotation = Quaternion.Euler(new Vector3(-YStrength, 0.0f, 0.0f) + transform.localRotation.eulerAngles);
                }
                break;


            case ERotationAxis.Both:
                // Get the current rotation strength.
                XStrength = Input.GetAxis("Mouse X") * XSensitivity;
                YStrength = Input.GetAxis("Mouse Y") * YSensitivity;

                // if the user has a camera attached, rotate the camera based on the speficied axis.
                if (Cam)
                {
                    switch (CamRotationAxis)
                    {
                        // If the user has the X axis specified for the camera.
                        case ECamRotationAxis.X:
                            Cam.localRotation = Quaternion.Euler(new Vector3(0.0f, XStrength, 0.0f) + Cam.localRotation.eulerAngles);
                            transform.localRotation = Quaternion.Euler(new Vector3(-YStrength, 0.0f, 0.0f) + transform.localRotation.eulerAngles);
                            break;


                        // If the user has the Y axis specified for the camera.
                        case ECamRotationAxis.Y:
                            Cam.localRotation = Quaternion.Euler(new Vector3(-YStrength, 0.0f, 0.0f) + Cam.localRotation.eulerAngles);
                            transform.localRotation = Quaternion.Euler(new Vector3(0.0f, XStrength, 0.0f) + transform.localRotation.eulerAngles);
                            break;
                    }
                }
                else
                {
                    // If the user does not have a camera attached.
                    transform.localRotation = Quaternion.Euler(new Vector3(-YStrength, XStrength, 0.0f) + transform.localRotation.eulerAngles);
                }
                break;
        }

    }
}
