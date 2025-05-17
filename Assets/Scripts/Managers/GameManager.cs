using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using MyGame.Managers;    // ← 추가: ResourceManager, UIManager, StageManager 참조용

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("초기 자원 설정")]
    [SerializeField] private int startCoins = 200;
    [SerializeField] private int startLives = 3;

    public int SelectedStage { get; private set; }

    [SerializeField] private CanvasGroup loadingCanvas;
    [SerializeField] private float fadeDuration = 0.5f;

    void Awake()
    {
        Debug.Log("[GameManager] Awake() running on " + gameObject.name);
        
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void LoadStage(int stageIndex)
    {
        Debug.Log($"[GameManager] LoadStage() called with index={stageIndex}");
        SelectedStage = stageIndex;

        /// 씬 이름 매핑
        string sceneName;
        switch(stageIndex)
        {
            case 1: sceneName = "SceneOne"; break;
            default: sceneName = $"Level{stageIndex}"; break;
        }

        StopAllCoroutines();
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        if (loadingCanvas != null)
        {
            loadingCanvas.gameObject.SetActive(true);
            yield return StartCoroutine(FadeCanvas(loadingCanvas, 0f, 1f));
        }

        var op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        op.allowSceneActivation = false;
        while (op.progress < 0.9f)
            yield return null;

        op.allowSceneActivation = true;
        yield return op;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "SceneOne" || scene.name.StartsWith("Level"))
        {
            // Level 씬이 완전히 로드된 후에만 UI 초기화
            ResourceManager.Instance.ResetResources(startCoins, startLives);
            UIManager.Instance.UpdateCoins(ResourceManager.Instance.CurrentCoins);
            UIManager.Instance.UpdateLives(ResourceManager.Instance.CurrentLives);
            UIManager.Instance.UpdateWave(1, StageManager.Instance.TotalWaves);

            // Level 씬 진입 후 스테이지 매니저로 게임 시작 로직 위임
           StageManager.Instance.StartStage();
        }
    }

    private IEnumerator FadeCanvas(CanvasGroup cg, float from, float to)
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / fadeDuration;
            cg.alpha = Mathf.Lerp(from, to, t);
            yield return null;
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
