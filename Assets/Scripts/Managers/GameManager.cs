using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

     [Header("UI Buttons")]
    [SerializeField] private Button startButton;  // Start 버튼 참조

    [Header("StageManager에 넘길 Scene(Stage) Name")]
    [SerializeField] private string stageSelectStageName = "SelectStage";
    [SerializeField] private string mainGameStageName = "SceneOne";

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            // 현재 씬이 ‘GameStartScene’일 때만 계속 유지
            if (SceneManager.GetActiveScene().name == "GameStartScene")
                DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // 버튼이 제대로 할당됐는지 체크
        if (startButton == null)
        {
            startButton = GameObject.Find("Canvas/Start").GetComponent<Button>();
            Debug.Log("[GameManager] startButton을 Find로 할당했습니다.");
        }

        // 코드로 리스너 등록
        startButton.onClick.AddListener(GoToStageSelect);

        Debug.Log("[GameManager] Start()에서 버튼 리스너 등록 완료");
    }

    public void GoToStageSelect()
    {
         Debug.Log($"[GameManager] 실제 로드 시도 씬 이름: {stageSelectStageName}");

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
            Debug.LogWarning("LoadScene: stageName이 비어있습니다.");
            return;
        }
        SceneManager.LoadScene(stageName);
    }

    public void ExitScene()
    {
        Debug.Log("[GameManager] ExitScene 호출됨");
        StageManager.Instance.LoadStage(stageSelectStageName);
    }
}
