using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Pan Settings (Left Drag)")]
    [Tooltip("팬 속도 계수")]
    public float panSpeed = 1.0f;
    [Header("Pan Limits (World X/Z)")]
    [Tooltip("X축 이동 제한 (min, max)")]
    public Vector2 panXLimits = new Vector2(-10f, 10f);
    [Tooltip("Z축 이동 제한 (min, max)")]
    public Vector2 panZLimits = new Vector2(-10f, 10f);

    [Header("Orbit Settings (Right Drag)")]
    [Tooltip("Yaw 회전 속도 (수평)")]
    public float yawSpeed = 50f;
    [Tooltip("Pitch 회전 속도 (수직)")]
    public float pitchSpeed = 40f;
    // [Tooltip("Yaw 회전 각도 제한 (min, max)")]
    // public Vector2 yawLimits = new Vector2(-180f, 180f);
    [Tooltip("Pitch 각도 제한 (min, max)")]
    public Vector2 pitchLimits = new Vector2(15f, 60f);

    [Header("Zoom Settings (Scroll Wheel)")]
    [Tooltip("줌 속도 계수")]
    public float zoomSpeed = 10f;
    [Tooltip("카메라 Field of View 최소/최대 값")]
    public Vector2 fovLimits = new Vector2(15f, 90f);

    private Vector3 lastMousePos;
    private float currentPitch;
    private float currentYaw;
    private Camera cam;

    void Start()
    {
        // 초기 각도 설정(제한 범위 내로 클램프)
        Vector3 angles = transform.eulerAngles;
        // currentYaw = Mathf.Clamp(angles.y, yawLimits.x, yawLimits.y);
        currentYaw = angles.y;
        currentPitch = Mathf.Clamp(angles.x, pitchLimits.x, pitchLimits.y);

        // 카메라 컴포넌트
        cam = GetComponent<Camera>();
        if (cam == null) Debug.LogWarning("CameraController에 Camera 컴포넌트가 없습니다.");
    }

    void Update()
    {
        //–– Left-Click Drag: Pan ––//
        if (Input.GetMouseButtonDown(0)) lastMousePos = Input.mousePosition;
        if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - lastMousePos;
            Vector3 right = transform.right;
            Vector3 forward = Vector3.Cross(right, Vector3.up).normalized;
            transform.position += (-delta.x * right + -delta.y * forward) * panSpeed * Time.deltaTime;

            // 팬 제한 적용
            Vector3 pos = transform.position;
            pos.x = Mathf.Clamp(pos.x, panXLimits.x, panXLimits.y);
            pos.z = Mathf.Clamp(pos.z, panZLimits.x, panZLimits.y);
            transform.position = pos;

            lastMousePos = Input.mousePosition;
        }

        //–– Right-Click Drag: Orbit ––//
        if (Input.GetMouseButtonDown(1)) lastMousePos = Input.mousePosition;
        if (Input.GetMouseButton(1))
        {
            Vector3 delta = Input.mousePosition - lastMousePos;
            if (delta.sqrMagnitude > 0.0f)
            {
                float yawDelta = delta.x * yawSpeed * Time.deltaTime;
                float pitchDelta = -delta.y * pitchSpeed * Time.deltaTime;

                // 누적 각도 업데이트 및 제한
                // currentYaw = Mathf.Clamp(currentYaw + yawDelta, yawLimits.x, yawLimits.y);
                currentYaw = currentYaw + yawDelta;
                currentPitch = Mathf.Clamp(currentPitch + pitchDelta, pitchLimits.x, pitchLimits.y);

                // 실제 변화량 계산
                float applyYaw = currentYaw - transform.eulerAngles.y;
                float applyPitch = currentPitch - transform.eulerAngles.x;

                transform.Rotate(Vector3.up, applyYaw, Space.World);
                transform.Rotate(Vector3.right, applyPitch, Space.Self);

                lastMousePos = Input.mousePosition;
            }
        }

        //–– Scroll Wheel: Zoom ––//
        if (cam != null)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (Mathf.Abs(scroll) > Mathf.Epsilon)
            {
                cam.fieldOfView = Mathf.Clamp(cam.fieldOfView - scroll * zoomSpeed, fovLimits.x, fovLimits.y);
            }
        }
    }
}
