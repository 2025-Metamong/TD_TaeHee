using UnityEngine;

public class GameOver : MonoBehaviour
{
    public static GameOver Instance { get; private set; }
    [SerializeField] private GameObject stageFailPanel;

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
    public void ShowStageFailUI()
    {
        stageFailPanel.SetActive(true);
    }
}
