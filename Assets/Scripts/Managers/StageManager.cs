using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }

    //private MapData map; // 현재 맵 정보

    private int wave; // 현재 웨이브 수
    
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void gameStart()
    {
        // Tower manager 호출
        Debug.Log("Game Start!");
        //TowerManager.Instance.Initialize();
        //nextWave();
    }

    //public void waveStart()
    //{
    //    // monster manager 호출
    //    Debug.Log($"Wave {wave + 1} 시작");
    //    Monster_Manager.Instance.SpawnWave(map.getWaveData(wave));
    //}

    //public void reachFinish(Monster monster)
    //{
    //    // 몬스터가 Finish에 도착 -> takeDamage 호출
    //    Player.Instance.TakeDamage(monster.Damage);
    //}

    //public void waveClear()
    //{
    //    // 현재 웨이브 종료 -> showUpgradeOption 호출
    //    Debug.log($"Wave {wave + 1} 클리어");
    //    UIManager.Instance.ShowUpgradeOptions();

    //    wave++;

    //    // if (map정보의 총 wave수가 현재 wave수가 많아지면)
    //    // stageFinish 호출 (stageFinish(int 0:fail, 1:sucess))
    //    if (wave >= mapTotalWaves)
    //    {
    //        Debug.log("스테이지 클리어");
    //        GameManager.Instance.StageFInish(1, GetCurrentStageIndex());
    //        // 1 = success
    //    }
    //}

    //public void nextWave()
    //{
    //    // 다음 웨이브 시작
    //    // Monster Manager에 Map 데이터를 기반으로 몬스터 스폰 정보 넘겨주기
    //    if (curentWave < map.TotalWaves)
    //    {
    //        waveStart();
    //    }

    //    else
    //    {
    //        Debug.LogWarning("모든 웨이브 완료");
    //    }
    //}

    //public void gameOver()
    //{
    //    // 현재 스테이지 종료 -> 메인화면으로(scene 변경)
    //    Debug.log("게임 오버");
    //    GameManager.Instance.stageFinish(0, GetCurrentStageIndex());
    //    // 0 = fail
    //}

    //private int GetCurrentStageIndex()
    //{
    //    string currentScene = SceneManager.GetActiveScene().name;
    //    return GameManager.Instance.StageListIndexOf(currentScene);
    //}
}
