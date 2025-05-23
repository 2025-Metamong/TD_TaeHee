using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [Header("Pan Settings (Left Drag)")]
    public float panSpeed = 2.0f;
    public Vector2 panXLimits = new Vector2(-50f, 50f);
    public Vector2 panZLimits = new Vector2(-50f, 50f);

    [Header("Orbit Settings (Right Drag)")]
    public float yawSpeed = 50f;
    public float pitchSpeed = 40f;
    public Vector2 pitchLimits = new Vector2(15f, 60f);

    [Header("Zoom Settings (Scroll Wheel)")]
    public float zoomSpeed = 10f;
    public Vector2 fovLimits = new Vector2(15f, 90f);

    [Header("Top-Down Zoom Limits")]
    [Tooltip("Top-Down 모드에서 허용할 오쏘그래픽 사이즈(min, max)")]
    public Vector2 orthographicSizeLimits = new Vector2(5f, 20f);

    [Header("View Toggle Settings")]
    public Transform perspectivePoint;
    public Transform topDownPoint;
    public float orthographicSize = 10f;

    private Camera cam;
    private Vector3 lastMousePos;
    private float currentYaw;
    private float currentPitch;
    private bool isTopDown = false;

    // 3D 복원용
    private Quaternion savedRotation;
    private float savedFOV;

    void Start()
    {
        cam = GetComponent<Camera>();
        var angles = transform.eulerAngles;
        currentYaw   = angles.y;
        currentPitch = Mathf.Clamp(angles.x, pitchLimits.x, pitchLimits.y);

        // 초기 3D 위치/회전
        if (perspectivePoint != null)
        {
            transform.position = perspectivePoint.position;
            transform.rotation = perspectivePoint.rotation;
        }
        savedFOV = cam.fieldOfView;
        cam.orthographic = false;
    }

    void Update()
    {
        HandlePan();
        HandleOrbit();
        HandleZoom();

        if (Input.GetKeyDown(KeyCode.T))
            ViewToggle();
    }

    private void HandlePan()
    {
        if (Input.GetMouseButtonDown(0)) lastMousePos = Input.mousePosition;
        if (Input.GetMouseButton(0))
        {
            var delta   = (Vector3)Input.mousePosition - lastMousePos;
            var right   = transform.right;
            var forward = Vector3.Cross(right, Vector3.up).normalized;
            transform.position += (-delta.x * right + -delta.y * forward)
                                  * panSpeed * Time.deltaTime;

            var pos = transform.position;
            pos.x = Mathf.Clamp(pos.x, panXLimits.x, panXLimits.y);
            pos.z = Mathf.Clamp(pos.z, panZLimits.x, panZLimits.y);
            transform.position = pos;

            lastMousePos = Input.mousePosition;
        }
    }

    private void HandleOrbit()
    {
        if (Input.GetMouseButtonDown(1)) lastMousePos = Input.mousePosition;
        if (Input.GetMouseButton(1) && !isTopDown)
        {
            var delta = (Vector3)Input.mousePosition - lastMousePos;
            if (delta.sqrMagnitude > 0f)
            {
                float yawDelta   = delta.x * yawSpeed   * Time.deltaTime;
                float pitchDelta = -delta.y * pitchSpeed * Time.deltaTime;

                currentYaw   += yawDelta;
                currentPitch = Mathf.Clamp(currentPitch + pitchDelta, pitchLimits.x, pitchLimits.y);

                float applyYaw   = currentYaw   - transform.eulerAngles.y;
                float applyPitch = currentPitch - transform.eulerAngles.x;
                transform.Rotate(Vector3.up,   applyYaw,   Space.World);
                transform.Rotate(Vector3.right, applyPitch, Space.Self);

                lastMousePos = Input.mousePosition;
            }
        }
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) < Mathf.Epsilon) return;

        if (isTopDown)
        {
            // Top-Down 모드에서 orthographicSizeLimits 범위 안에서만 변경
            orthographicSize = Mathf.Clamp(
                orthographicSize - scroll * zoomSpeed,
                orthographicSizeLimits.x,
                orthographicSizeLimits.y
            );
            cam.orthographicSize = orthographicSize;
        }
        else
        {
            cam.fieldOfView = Mathf.Clamp(
                cam.fieldOfView - scroll * zoomSpeed,
                fovLimits.x,
                fovLimits.y
            );
            savedFOV = cam.fieldOfView;
        }
    }

    public void ViewToggle()
    {
        isTopDown = !isTopDown;

        if (isTopDown)
        {
            // 3D 상태 저장
            savedRotation = transform.rotation;
            savedFOV      = cam.fieldOfView;
            ApplyTopDown();
        }
        else
        {
            // 저장된 3D 상태 복원
            transform.rotation = savedRotation;
            cam.orthographic   = false;
            cam.fieldOfView    = savedFOV;
        }
    }

    private void ApplyTopDown()
    {
        transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        if (topDownPoint != null)
            transform.position = topDownPoint.position;

        cam.orthographic     = true;
        cam.orthographicSize = Mathf.Clamp(
            orthographicSize,
            orthographicSizeLimits.x,
            orthographicSizeLimits.y
        );
    }
}
