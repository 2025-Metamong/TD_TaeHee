using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class StageButton : MonoBehaviour
{
    [Tooltip("로드할 Scene 이름")]
    public int StageIndex=-1;
    private Button _button;

    void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClicked);
    }

    void OnDestroy()
    {
        _button.onClick.RemoveListener(OnClicked);
    }

    private void OnClicked()
    {
        if (StageIndex < 0)
        {
            Debug.LogWarning($"[{StageIndex}]에 할당된 StageData가 없거나 stageName이 비어있습니다.");
            return;
        }

        GameManager.Instance.LoadScene("InStage");
        StageManager.Instance.LoadStage(StageIndex);
    }
}
