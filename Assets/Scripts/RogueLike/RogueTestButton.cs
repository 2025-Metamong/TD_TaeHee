using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class RogueTestButton : MonoBehaviour
{

    [Header("업그레이드 시스템")]
    public RoguelikeUpgrade roguelikeUpgrade;
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
        if (roguelikeUpgrade == null)
        {
            Debug.LogWarning("UIManager: RoguelikeUpgrade 레퍼런스가 없습니다!");
            return;
        }
        roguelikeUpgrade.ShowUpgradeMenu();
    }
}