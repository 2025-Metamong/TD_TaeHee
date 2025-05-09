using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private List<string> stageList = new List<string>();
    // 게임 내에 있는 stage list (stage자체는 scene으로 구분)

    private int clearStageNum = 0;
    // 현재까지 clear한 마지막 stage num

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
        // Stage loading -> scene변경, stage manager호출
        if (stage >= 0 && stage < stageList.Count)
        {
            string sceneName = stageList[stage];
            SceneManager.LoadScene(sceneName);
        }

        else
        {
            Debug.LogWarning("잘못된 스테이지 인덱스입니다.");
        }
    }

    public void stageFinish(int N, int stage)
    {
        // 메인화면으로(scene 변경)
        // -> 현재 클리어한 stage가 clearStageNum이랑 같으면
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


