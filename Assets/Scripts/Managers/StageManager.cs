using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using MyGame.Managers;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }

    [Header("�ε� ���̵� ����")]
    [SerializeField] private CanvasGroup fadeCanvasGroup;  // ȭ�� ��ü�� ���� CanvasGroup
    [SerializeField] private float fadeDuration = 0.5f;    // ���̵� ��/�ƿ� �ð�

    void Awake()
    {
        // �̱��� ����
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // ���ʿ��� ���̵� �ƿ� ���·�
        if (fadeCanvasGroup != null)
            fadeCanvasGroup.alpha = 0f;
    }

    /// <summary>
    /// �ܺο��� ȣ���� ��(��������) �ε� �Լ�
    /// </summary>
    public void LoadStage(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogWarning($"StageManager.LoadStage: sceneName�� ����ֽ��ϴ�.");
            return;
        }
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    /// <summary>
    /// ���̵� �� �� �񵿱� �ε� �� ���̵� �ƿ� �帧
    /// </summary>
    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        // 1) ���̵� ��
        if (fadeCanvasGroup != null)
        {
            yield return StartCoroutine(Fade(0f, 1f));
        }

        // 2) �񵿱� �� �ε�
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;

        // �ε��� ���� ������ ���
        while (op.progress < 0.9f)
            yield return null;

        // �Ϸ� ǥ�� �� �ٷ� Ȱ��ȭ
        op.allowSceneActivation = true;
        while (!op.isDone)
            yield return null;

        // 3) ���̵� �ƿ�
        if (fadeCanvasGroup != null)
        {
            yield return StartCoroutine(Fade(1f, 0f));
        }
    }

    /// <summary>
    /// CanvasGroup alpha�� from��to�� ����
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

    private void Update()
    {

    }

    // for testing
    int currentStage = 0;
    int totalWave = 2;
    
    // for testing

    private bool waveFlag = false; // wave flag (false = ready, true = wave start)

    public void SetFlag(bool value)
    {
        if (value)
        {
            Debug.Log("Wave Start");
            monsterManager.StartWave(currentWave);
            waveFlag = true;
        }
    }

    public void FinishWave()
    {
        waveFlag = false;
        currentStage += 1;
        Debug.Log("Wave End");
    }


}