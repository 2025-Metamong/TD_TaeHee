using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class RoguelikeUpgrade : MonoBehaviour
{
    [Header("UI 참조")]
    public GameObject upgradeMenuPanel;      // 업그레이드 창 루트 (비활성화 상태로 에디터에 둠)
    public Button[] optionButtons = new Button[3];  // 3개의 선택 버튼

    [Header("업그레이드 데이터")]
    public List<RogueUpgrade> allUpgrades;    // ScriptableObject 등으로 정의한 업그레이드 목록

    private CanvasGroup _canvasGroup;

    void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        upgradeMenuPanel.SetActive(false);
    }

    // 업그레이드 메뉴를 화면에 띄우고, 3가지 랜덤 옵션을 버튼에 세팅한다.
    public void ShowUpgradeMenu()
    {
        // 메뉴 보이기
        upgradeMenuPanel.SetActive(true);
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;

        // 랜덤으로 3개 고르기
        var choices = allUpgrades.OrderBy(u => Random.value).Take(3).ToArray();

        for (int i = 0; i < optionButtons.Length; i++)
        {
            var btn = optionButtons[i];
            var data = choices[i];
            
            // 텍스트 세팅 (TextMeshProUGUI 쓰고 있다면 TMP 컴포넌트로 교체)
            btn.GetComponentInChildren<Text>().text = data.upgradeName;
            
            // 기존 리스너 제거 후 새로 추가
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => OnUpgradeSelected(data));
            
            btn.gameObject.SetActive(true);
        }
    }

    private void OnUpgradeSelected(RogueUpgrade selected)
    {
        // 실제 업그레이드 적용 로직
        ApplyUpgrade(selected);
        
        // 메뉴 숨기기
        upgradeMenuPanel.SetActive(false);
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }

    private void ApplyUpgrade(RogueUpgrade data)
    {
        // 예: 플레이어 스탯 증가, 무기 강화 등
        Debug.Log($"업그레이드 적용: {data.upgradeName}");
        // TODO: 실제 게임 로직 추가
    }
}
