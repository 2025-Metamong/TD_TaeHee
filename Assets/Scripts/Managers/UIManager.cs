using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("HUD Elements")]
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI enemiesText;       // ← 추가: 남은 몬스터 수 표시용 Text

    [Header("Panels")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject stageClearPanel;

    void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }

    // HUD 업데이트
    public void UpdateCoins(int amount)
    {
        coinsText.text = amount.ToString();
    }

    public void UpdateLives(int lives)
    {
        livesText.text = lives.ToString();
    }

    public void UpdateWave(int currentWave, int totalWaves)
    {
        waveText.text = $"Wave {currentWave}/{totalWaves}";
    }

    /// <summary>
    /// 남아있는 몬스터 수를 HUD에 표시합니다.
    /// </summary>
    public void UpdateRemainingEnemies(int count)
    {
        if (enemiesText != null)
            enemiesText.text = $"Enemies: {count}";
    }

    // 게임 오버 UI
    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
    }

    // 스테이지 클리어 UI
    public void ShowStageClear()
    {
        stageClearPanel.SetActive(true);
    }
}
