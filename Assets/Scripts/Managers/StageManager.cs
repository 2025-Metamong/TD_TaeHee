using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using MyGame.Managers;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }

    [Header("Player Status")]
    [Tooltip("Player's max Health")]
    public int maxHealth = 10;

    [Tooltip("Player's Health")]
    public int Health { get; private set; }

    [Header("Money")]
    [Tooltip("Player's Coin")]
    public int Coin { get; private set; }


    [Header("로딩 페이드 설정")]
    [SerializeField] private CanvasGroup fadeCanvasGroup;  // 화면 전체를 덮는 CanvasGroup
    [SerializeField] private float fadeDuration = 0.5f;    // 페이드 인/아웃 시간

    // by seungwon
    [Header("stage info")]
    [SerializeField] private List<StageInfo> stageInfoList = new List<StageInfo>();

    [SerializeField] private MonsterManager monsterManager;
    [SerializeField] private TowerManager towerManager;

    public int currentWave = 0;

    // 마지막 클리어 스테이지 번호
    int lastClearStage = 0;
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

        Health = maxHealth;


        // 최초에는 페이드 아웃 상태로
        if (fadeCanvasGroup != null)
            fadeCanvasGroup.alpha = 0f;
    }
    private void Start()
    {
        monsterManager = MonsterManager.Instance;
        towerManager = TowerManager.Instance;
    }

    // 외부(몬스터 등)에서 호출하는 데미지 처리 메서드
    public void TakeDamage(int damage)
    {
        Health -= damage;
        Debug.Log($"플레이어가 {damage}만큼 피해. 남은 체력: {Health}");

        if (Health <= 0)
        {
            Health = 0;
            OnGameOver();
        }
    }

    private void OnGameOver()
    {
        Debug.Log("Game Over!");
        SceneManager.LoadScene("SelectStage");
    }

    // 외부에서 호출할 씬(스테이지) 로드 함수

    public void LoadStage(int stageIndex = -1)
    {
        Debug.Log($"[LoadStage] idx={stageIndex}, infos={stageInfoList?.Count}, monsterMngr={(monsterManager==null?"NULL":"OK")}, towerMngr={(towerManager==null?"NULL":"OK")}");
        if (stageIndex < 0 || stageIndex >= stageInfoList.Count)
        {
            Debug.LogWarning("StageManager.LoadStage: 잘못된 인덱스");
            return;
        }

        if (!Application.isPlaying) return;

        if (stageIndex < 0)
        {

            Debug.LogWarning("StageManager.LoadStage: wrong stage index");
            return;
        }
        currentWave = 0;

        // 초기 코인 & 체력 세팅
        Debug.Log($"StageManager.LoadStage: 코인={stageInfoList[stageIndex].startCoins}, 체력={stageInfoList[stageIndex].playerHP}");
        Coin = stageInfoList[stageIndex].startCoins;
        Health = stageInfoList[stageIndex].playerHP;

        StartCoroutine(LoadSceneCoroutine(stageIndex));

        this.currentStage = stageIndex;

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
        RoguelikeManager.Instance.ResetRoguelikeUpgrade();
    }

    // CanvasGroup alpha를 from→to로 변경
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
        if (currentWave < stageInfoList[currentStage].monsterSpawnList.Count)
            RoguelikeManager.Instance.ShowUpgradeMenu();
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
        lastClearStage = currentStage > lastClearStage ? currentStage : lastClearStage;
    }

    public bool UseCoin(int amount)
    {
        if (amount <= 0) return true;    // 0 이하 사용은 항상 OK
        if (Coin >= amount)
        {
            Coin -= amount;
            Debug.Log($"코인 사용: {amount} ({Coin} 남음)");
            return true;
        }
        else
        {
            Debug.LogWarning($"코인 부족: {amount} 필요, 현재 {Coin}");
            return false;
        }
    }
    
    // 코인 추가
    public void AddCoins(int amount)
    {
        if (amount <= 0) return;
        Coin += amount;
        Debug.Log($"코인 획득: {amount} (총 {Coin})");
    }

}