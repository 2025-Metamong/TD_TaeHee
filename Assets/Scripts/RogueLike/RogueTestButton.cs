using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class RogueTestButton : MonoBehaviour
{

    [Header("업그레이드 시스템")]
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
        if (RoguelikeManager.Instance == null)
        {
            Debug.LogWarning("RogueTestButton: RoguelikeManager 인스턴스가 없습니다!");
            return;
        }
        RoguelikeManager.Instance.ShowUpgradeMenu();
    }
}