using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("StageManager에 넘길 씬(스테이지) 이름")]
    [SerializeField] private string stageSelectStageName = "StageSelectScene";
    [SerializeField] private string mainGameStageName = "MainGameScene";

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void GoToStageSelect()
    {
        if (string.IsNullOrEmpty(stageSelectStageName))
        {
            Debug.LogWarning("GoToStageSelect: stageSelectStageName이 비어있습니다");
            return;
        }

        StageManager.Instance.LoadStage(stageSelectStageName);
    }
    public void StartGame()
    {
        if (string.IsNullOrEmpty(mainGameStageName))
        {
            Debug.LogWarning("StartGame: mainGameStageName이 비어있습니다.");
            return;
        }
        StageManager.Instance.LoadStage(mainGameStageName);
    }

    public void LoadScene(string stageName)
    {
        if (string.IsNullOrEmpty(stageName))
        {
            Debug.LogWarning("LoadScene: sceneName이 비어있습니다.");
            return;
        }
        SceneManager.LoadScene(stageName);
    }
}
