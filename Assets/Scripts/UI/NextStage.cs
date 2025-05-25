using MyGame.UI;
using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

public class NextStage : MonoBehaviour
{
    public static NextStage Instance { get; private set; }

    private Button _button;
    int currentStage;
    List<StageInfo> stageInfoList;

    void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClicked);
    }

    void OnEnable()
    {
        currentStage = StageManager.Instance.getCurrentStage;
        stageInfoList = StageManager.Instance.getStageList;
    }

    private void OnClicked()
    {
        if (currentStage >= stageInfoList.Count - 1)
        {
            Debug.LogWarning("���� ���������� �����ϴ�. ������ ���������Դϴ�.");
            return;
        }
        StageManager.Instance.ExitStage();
        GameManager.Instance.LoadScene("SelectStage");
        GameManager.Instance.LoadScene("InStage");
        StageManager.Instance.LoadStage(currentStage+1);
    }

}
