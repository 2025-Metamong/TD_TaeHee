using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using MyGame.Managers;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }

    [Header("로딩 페이드 설정")]
    [SerializeField] private CanvasGroup fadeCanvasGroup;  // 화면 전체를 덮는 CanvasGroup
    [SerializeField] private float fadeDuration = 0.5f;    // 페이드 인/아웃 시간

    void Awake()
    {
        // 싱글톤 설정
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // 최초에는 페이드 아웃 상태로
        if (fadeCanvasGroup != null)
            fadeCanvasGroup.alpha = 0f;
    }

    /// <summary>
    /// 외부에서 호출할 씬(스테이지) 로드 함수
    /// </summary>
    public void LoadStage(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogWarning($"StageManager.LoadStage: sceneName이 비어있습니다.");
            return;
        }
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    /// <summary>
    /// 페이드 인 → 비동기 로드 → 페이드 아웃 흐름
    /// </summary>
    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        // 1) 페이드 인
        if (fadeCanvasGroup != null)
        {
            yield return StartCoroutine(Fade(0f, 1f));
        }

        // 2) 비동기 씬 로드
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;

        // 로딩이 끝날 때까지 대기
        while (op.progress < 0.9f)
            yield return null;

        // 완료 표시 후 바로 활성화
        op.allowSceneActivation = true;
        while (!op.isDone)
            yield return null;

        // 3) 페이드 아웃
        if (fadeCanvasGroup != null)
        {
            yield return StartCoroutine(Fade(1f, 0f));
        }
    }

    /// <summary>
    /// CanvasGroup alpha를 from→to로 변경
    /// </summary>
    private IEnumerator Fade(float from, float to)
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / fadeDuration);
            fadeCanvasGroup.alpha = Mathf.Lerp(from, to, t);
            yield return null;
        }
        fadeCanvasGroup.alpha = to;
    }

    // by seungwon
    [Header("stage info")]
    [SerializeField]  private List<StageInfo> stageInfoList = new List<StageInfo>();
    public MonsterManager monsterManager;
    public int currentWave = 0;

    // for testing
    int currentStage = 0;
    int totalWave = 2;
    
    // for testing

    private bool waveFlag = false; // wave flag (false = ready, true = wave start)

    public void SetFlag(bool value)
    {
        if (value && waveFlag == false)
        {
            Debug.Log("Stage Manager : Wave Start");
            monsterManager.StartWave(currentWave);
            waveFlag = true;
        }
    }

    public void FinishWave()
    {
        waveFlag = false;
        currentWave += 1;
        Debug.Log("Wave End");
    }


}