using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float bottomClamp = -40f;
    [SerializeField] private float topClamp = 70f;

    private float cinemachineTargetPitch;
    private float cinemachineTargetYaw;

    private void LateUpdate()
    {
        CameraLogic();
    }

    private void CameraLogic()
    {
        float mouseX = GetMouseInput("Mouse X");
        float mouseY = GetMouseInput("Mouse Y");

        cinemachineTargetPitch = UpdateRotation(cinemachineTargetPitch, mouseY, bottomClamp, topClamp, true);
        cinemachineTargetYaw = UpdateRotation(cinemachineTargetYaw, mouseX, float.MinValue, float.MaxValue, false);

        ApplyRotations(cinemachineTargetPitch, cinemachineTargetYaw);
    }

    private void ApplyRotations(float pitch, float yaw)
    {
        target.rotation = Quaternion.Euler(pitch, yaw, target.eulerAngles.z);
    }

    private float UpdateRotation(float currentRotation, float input, float min, float max, bool isXAxis)
    {
        currentRotation += isXAxis ?-input : input;
        return Mathf.Clamp(currentRotation, min, max);
    }

    private float GetMouseInput(string axis)
    {
        return Input.GetAxis(axis) * rotationSpeed * Time.deltaTime;
    }

}
