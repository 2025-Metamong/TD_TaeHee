using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }

    //private MapData map; // ���� �� ����

    private int wave; // ���� ���̺� ��
    
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void gameStart()
    {
        // Tower manager ȣ��
        Debug.Log("Game Start!");
        //TowerManager.Instance.Initialize();
        //nextWave();
    }

    //public void waveStart()
    //{
    //    // monster manager ȣ��
    //    Debug.Log($"Wave {wave + 1} ����");
    //    Monster_Manager.Instance.SpawnWave(map.getWaveData(wave));
    //}

    //public void reachFinish(Monster monster)
    //{
    //    // ���Ͱ� Finish�� ���� -> takeDamage ȣ��
    //    Player.Instance.TakeDamage(monster.Damage);
    //}

    //public void waveClear()
    //{
    //    // ���� ���̺� ���� -> showUpgradeOption ȣ��
    //    Debug.log($"Wave {wave + 1} Ŭ����");
    //    UIManager.Instance.ShowUpgradeOptions();

    //    wave++;

    //    // if (map������ �� wave���� ���� wave���� ��������)
    //    // stageFinish ȣ�� (stageFinish(int 0:fail, 1:sucess))
    //    if (wave >= mapTotalWaves)
    //    {
    //        Debug.log("�������� Ŭ����");
    //        GameManager.Instance.StageFInish(1, GetCurrentStageIndex());
    //        // 1 = success
    //    }
    //}

    //public void nextWave()
    //{
    //    // ���� ���̺� ����
    //    // Monster Manager�� Map �����͸� ������� ���� ���� ���� �Ѱ��ֱ�
    //    if (curentWave < map.TotalWaves)
    //    {
    //        waveStart();
    //    }

    //    else
    //    {
    //        Debug.LogWarning("��� ���̺� �Ϸ�");
    //    }
    //}

    //public void gameOver()
    //{
    //    // ���� �������� ���� -> ����ȭ������(scene ����)
    //    Debug.log("���� ����");
    //    GameManager.Instance.stageFinish(0, GetCurrentStageIndex());
    //    // 0 = fail
    //}

    //private int GetCurrentStageIndex()
    //{
    //    string currentScene = SceneManager.GetActiveScene().name;
    //    return GameManager.Instance.StageListIndexOf(currentScene);
    //}
}
