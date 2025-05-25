using UnityEngine;

// World Space Canvas를 빌보드처럼 카메라를 항상 바라보게 만드는 스크립트

[RequireComponent(typeof(RectTransform))]
public class BillBoardUI : MonoBehaviour
{
    [Header("빌보드 대상 카메라")]
    [SerializeField, Tooltip("빌보드 효과를 적용할 카메라를 할당하세요. 비워두면 Main Camera를 자동으로 참조합니다.")]
    private Camera targetCamera;

    // RectTransform을 캐싱하여 매 프레임 GetComponent 호출을 방지합니다.
    private RectTransform canvasRectTransform;

    private void Awake()
    {
        // 컴포넌트 캐싱: 성능 최적화를 위해 Awake에서 한번만 호출
        canvasRectTransform = GetComponent<RectTransform>();

        // Inspector에 카메라가 지정되지 않았으면 Main Camera 자동 할당
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }
    }

    private void LateUpdate()
    {
        // LateUpdate에서 카메라 회전값을 가져와서 UI Canvas 회전에 그대로 적용
        // 이 방식은 UI가 카메라와 동일한 방향을 유지하도록 보장합니다.
        canvasRectTransform.rotation = targetCamera.transform.rotation;
    }
}
