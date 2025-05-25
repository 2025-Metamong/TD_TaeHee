using UnityEngine;

public class StageClearPanle : MonoBehaviour
{
    public static StageClearPanle Instance { get; private set; }
    [SerializeField] private GameObject stageEndPanel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 스테이지 클리어 UI 표시
    public void ShowStageEndUI()
    {
        stageEndPanel.SetActive(true);
    }
}
