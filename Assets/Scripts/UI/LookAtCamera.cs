using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public enum CameraMode
    {
        None,
        LookAtCamera
    }

    [SerializeField] private CameraMode mode;

    private void Update()
    {
        switch (mode)
        {
            default:
            case CameraMode.None:
                break;
            case CameraMode.LookAtCamera:
                transform.LookAt(Camera.main.transform.position);
                break;
        }
    }
}
