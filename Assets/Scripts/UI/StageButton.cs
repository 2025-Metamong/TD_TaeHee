using UnityEngine;
using UnityEngine.UI;
using MyGame.Managers;    // GameManager가 여기에 정의되어 있습니다

[RequireComponent(typeof(Button))]
public class StageButton : MonoBehaviour
{
    [Tooltip("이 버튼이 로드할 스테이지 번호 (1부터 시작)")]
    [SerializeField] private int stageIndex = 1;

    private Button _button;

    void Awake()
    {
        _button = GetComponent<Button>();
        Debug.Log("[StageButton] Awake: found Button = " + _button);
        _button.onClick.AddListener(OnClicked);
    }

    void OnDestroy()
    {
        _button.onClick.RemoveListener(OnClicked);
    }

    private void OnClicked()
{
    Debug.Log("[StageButton] OnClicked fired for index " + stageIndex);
    
    // 매니저 확인
        if (StageManager.Instance == null || GameManager.Instance == null) return;

    int maxStage = StageManager.Instance.TotalWaves;

    // TotalWaves가 0이면 스테이지 정보가 아직 로드 전이므로 검사하지 않음
    if (maxStage > 0 && (stageIndex < 1 || stageIndex > maxStage))
    {
        Debug.LogWarning($"[StageButton] 잘못된 스테이지 인덱스: {stageIndex} (허용 범위: 1~{maxStage})");
        return;
    }
    GameManager.Instance.LoadStage(stageIndex);
}

}
