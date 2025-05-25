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
            Debug.LogWarning("다음 스테이지가 없습니다. 마지막 스테이지입니다.");
            return;
        }
        StageManager.Instance.ExitStage();
        GameManager.Instance.ExitScene();
        GameManager.Instance.LoadScene("InStage");
        StageManager.Instance.LoadStage(currentStage+1);
    }

}
