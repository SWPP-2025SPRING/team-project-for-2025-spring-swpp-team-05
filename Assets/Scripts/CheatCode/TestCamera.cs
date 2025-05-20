using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCamera : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotateSpeed = 70f;
    public float zoomSpeed = 5f;

    public float positionLerpSpeed = 10f;
    public float rotationLerpSpeed = 10f;
    public float zoomLerpSpeed = 10f;

    private Vector3 targetPosition;
    private float targetYaw;
    private float targetPitch;
    private float targetZoom;

    private Camera cam;

    void Start()
    {
        targetPosition = transform.position;
        targetYaw = transform.eulerAngles.y;
        targetPitch = transform.eulerAngles.x;

        cam = GetComponent<Camera>();
        if (cam == null) cam = Camera.main;

        targetZoom = cam.fieldOfView;
    }

    void Update()
    {
        HandleInput();
        ApplyInterpolation();
    }

    void HandleInput()
    {
        // --- 이동 입력 (WASD) ---
        Vector3 moveDir = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) moveDir += transform.forward;
        if (Input.GetKey(KeyCode.S)) moveDir -= transform.forward;
        if (Input.GetKey(KeyCode.D)) moveDir += transform.right;
        if (Input.GetKey(KeyCode.A)) moveDir -= transform.right;

        targetPosition += moveDir.normalized * moveSpeed * Time.deltaTime;

        // --- 회전 입력 (Q/E: Yaw, R/F: Pitch) ---
        if (Input.GetKey(KeyCode.Q)) targetYaw -= rotateSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.E)) targetYaw += rotateSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.R)) targetPitch -= rotateSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.F)) targetPitch += rotateSpeed * Time.deltaTime;
        targetPitch = Mathf.Clamp(targetPitch, -89f, 89f);

        // --- 줌 입력 (T/G) ---
        if (Input.GetKey(KeyCode.T)) targetZoom -= zoomSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.G)) targetZoom += zoomSpeed * Time.deltaTime;
        targetZoom = Mathf.Clamp(targetZoom, 15f, 90f);
    }

    void ApplyInterpolation()
    {
        // --- 위치 보간 ---
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * positionLerpSpeed);

        // --- 회전 보간 ---
        Quaternion targetRot = Quaternion.Euler(targetPitch, targetYaw, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * rotationLerpSpeed);

        // --- 줌 보간 ---
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetZoom, Time.deltaTime * zoomLerpSpeed);
    }
}