using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public enum CameraMode
    {
        None,
        LookAt,
        LookAtInverted,
        CameraForward,
        CameraForwardInverted
    }

    [SerializeField] private CameraMode mode;

    private void LateUpdate()
    {
        switch (mode)
        {
            default:
            case CameraMode.None:
                break;
            case CameraMode.LookAt:
                transform.LookAt(Camera.main.transform);
                break;
            case CameraMode.LookAtInverted:
                Vector3 dirFromCamera = transform.position - Camera.main.transform.position;
                transform.LookAt(transform.position + dirFromCamera);
                break;
            case CameraMode.CameraForward:
                transform.forward = Camera.main.transform.forward;
                break;
            case CameraMode.CameraForwardInverted:
                transform.forward = -Camera.main.transform.forward;
                break;
        }
    }
}
