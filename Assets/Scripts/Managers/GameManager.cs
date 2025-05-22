using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int LastClearStage { get; private set; }

    private Button startButton;
    private Button exitButton;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            // 현재 씬이 ‘GameStart’일 때만 계속 유지
            if (SceneManager.GetActiveScene().name == "GameStart")
                DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    /// <summary>
    /// 외부에서 마지막 클리어 스테이지를 설정
    /// </summary>
    public void SetLastClearStage(int stageIndex)
    {
        LastClearStage = stageIndex;
        Debug.Log($"[GameManager] Last clear stage set to {stageIndex}");
        // 필요하다면 PlayerPrefs 에 저장
        PlayerPrefs.SetInt("LastClearStage", stageIndex);
        PlayerPrefs.Save();
    }

    void Start()
    {
        LastClearStage = PlayerPrefs.GetInt("LastClearStage", 0);
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"[GameManager] OnSceneLoaded 호출: scene.name = {scene.name}");

        if (scene.name == "GameStart")
        {
            // 버튼 찾기
            startButton = GameObject.Find("Canvas/Start")?.GetComponent<Button>();
            if (startButton != null)
            {
                startButton.onClick.RemoveAllListeners();
                startButton.onClick.AddListener(GoToStageSelect);
            }
        }
        else if (scene.name == "SceneOne")
        {
            Debug.Log("[GameManager] SceneOne 분기 진입 (하드코딩)");
            exitButton = GameObject.Find("Canvas/Exit")?.GetComponent<Button>();
            Debug.Log($"[GameManager] ExitButton 찾음? {(exitButton!=null)}");
            if (exitButton == null)
            {
                Debug.LogError("[GameManager] SceneOne에서 Exit 버튼을 찾을 수 없습니다. 경로를 확인하세요.");
                return;
            }

            exitButton.onClick.RemoveAllListeners();
            exitButton.onClick.AddListener(ExitScene);
            Debug.Log("[GameManager] SceneOne 로드 직후 Exit 버튼 리스너 등록 완료");
        }

    }

    public void GoToStageSelect()
    {
        const string target = "SelectStage";
        Debug.Log($"[GameManager] GotoStageSelect: {target}");
        StageManager.Instance.LoadStage(target);
    }

    public void StartGame()
    {
        StageManager.Instance.LoadStage("SceneOne");
    }

    public void LoadScene(string stageName)
    {
        if (string.IsNullOrEmpty(stageName))
        {
            Debug.LogWarning("LoadScene: stageName이 비어있습니다.");
            return;
        }
        Debug.Log($"[StageManager] 로드 시도: {stageName}");
        SceneManager.LoadScene(stageName);
    }

    public void ExitScene()
    {
        const string target = "SelectStage";
        Debug.Log($"[GameManager] ExitScene: {target}");
        StageManager.Instance.LoadStage(target);
    }
}
