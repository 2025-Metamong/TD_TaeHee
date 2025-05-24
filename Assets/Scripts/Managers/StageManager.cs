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

    // by seungwon
    [Header("stage info")]
    [SerializeField] private List<StageInfo> stageInfoList = new List<StageInfo>();
    [SerializeField] private MonsterManager monsterManager;
    public int currentWave = 0;

    // 마지막 클리어 스테이지 번호
    int currentStage = 0;

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
    public void LoadStage(int stageIndex = -1)
    {
        if (stageIndex < 0)
        {

            Debug.LogWarning($"StageManager.LoadStage: sceneName이 비어있습니다.");

            return;
        }
        this.currentWave = 0;
        StartCoroutine(LoadSceneCoroutine(stageIndex));
        monsterManager.SetMonsterManagerStageInfo(stageInfoList[stageIndex]);
    }

    /// <summary>
    /// 페이드 인 → 비동기 로드 → 페이드 아웃 흐름
    /// </summary>
    private IEnumerator LoadSceneCoroutine(int stageIndex)
    {
        // 1) 페이드 인
        if (fadeCanvasGroup != null)
        {
            yield return StartCoroutine(Fade(0f, 1f));
        }

        // 2) 비동기 씬 로드
        AsyncOperation op = SceneManager.LoadSceneAsync("InStage");
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

        // Loading Map Prefab
        Instantiate(stageInfoList[stageIndex].map, this.transform);

        // Camera 이동 범위 세팅
        var camControl = GameObject.FindWithTag("MainCamera")?.GetComponent<CameraController>();
        camControl?.SetCameraPanLimits(stageInfoList[stageIndex].panXLimits,
                                       stageInfoList[stageIndex].panZLimits);
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

    // for testing

    private bool waveFlag = false; // wave flag (false = ready, true = wave start)

    void Start()
    {
        monsterManager = MonsterManager.Instance;
    }

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

    public void FinishStage()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.name.StartsWith("stageMap"))
            {
                waveFlag = false;
                Destroy(child.gameObject);
            }
        }
    }

}