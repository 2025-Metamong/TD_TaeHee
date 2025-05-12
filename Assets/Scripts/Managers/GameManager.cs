using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private List<string> stageList = new List<string>();
    // ���� ���� �ִ� stage list (stage��ü�� scene���� ����)

    private int clearStageNum = 0;
    // ������� clear�� ������ stage num

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void stageSelect(int stage) 
    {
        // Stage loading -> scene����, stage managerȣ��
        if (stage >= 0 && stage < stageList.Count)
        {
            string sceneName = stageList[stage];
            SceneManager.LoadScene(sceneName);
        }

        else
        {
            Debug.LogWarning("�߸��� �������� �ε����Դϴ�.");
        }
    }

    public void stageFinish(int N, int stage)
    {
        // ����ȭ������(scene ����)
        // -> ���� Ŭ������ stage�� clearStageNum�̶� ������
        // clearStageNum += N
        if(stage == clearStageNum)
        {
            clearStageNum += N;
        }

        SceneManager.LoadScene("MainMenu");
    }

    public int GetClearStageNum()
    {
        return clearStageNum;
    }
}


