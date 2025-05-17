using System.Collections;
using UnityEngine;

// GameObject에 이 컴포넌트가 있어야 호출이 가능함.
[RequireComponent(typeof(LineRenderer))]
public class RangeIndicator : MonoBehaviour
{
    // 그려야할 원 계산하고, 원 표출하는 스크립트.
    [SerializeField, Tooltip("원 분할 수")] private int segments = 60;
    private LineRenderer lineRenderer;
    private float range;
    private Coroutine showRoutine;


    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true;
        lineRenderer.loop = true;
        lineRenderer.positionCount = segments + 1;
        lineRenderer.enabled = false;

        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        lineRenderer.startColor = new Color(1f, 0f, 0f, 1f);  // 완전 불투명한 빨간색
        lineRenderer.endColor   = new Color(1f, 0f, 0f, 1f);
    }

    /// <summary>
    /// 외부에서 사거리값을 초기화하고, 원을 계산. 사거리 표기하고 싶은 부모가 호출해두면 좋을 것.
    /// </summary>
    public void Initialize(float range)
    {
        this.range = range;
        // 원 둘레를 그리는 각도 단위
        float deltaTheta = (2f * Mathf.PI) / segments;
        for (int i = 0; i <= segments; i++)
        {
            float theta = deltaTheta * i;
            float x = Mathf.Cos(theta) * range;
            float z = Mathf.Sin(theta) * range;
            lineRenderer.SetPosition(i, new Vector3(x, 0f, z));
        }
    }

    // 정해진 시간 동안 보여줬다가 숨기기.
    public void ShowForSeconds(float seconds)
    {
        // 이미 표시 중인 코루틴이 있으면 중단
        if (showRoutine != null)
            StopCoroutine(showRoutine);

        showRoutine = StartCoroutine(ShowCoroutine(seconds));
    }

    // 정해진 시간만큼 수행하기 위한 코루틴.
    private IEnumerator ShowCoroutine(float seconds)
    {
        Debug.Log($"사거리 표시 for {seconds}");
        lineRenderer.enabled = true;  // 표시
        yield return new WaitForSeconds(seconds);  // 정해진 초 만큼 대기.
        lineRenderer.enabled = false; // 숨기기
        showRoutine = null;     // 코루틴 수행 끝남.
        Debug.Log($"사거리 표시 끝");
    }


    // 원 표출 함수
    public void Show()
    {
        lineRenderer.enabled = true;
    }

    // 원 숨기는 함수
    public void Hide()
    {
        lineRenderer.enabled = false;
    }

    // 원을 토글하는 함수
    public void Toggle()
    {
        lineRenderer.enabled = !lineRenderer.enabled;
    }
}
