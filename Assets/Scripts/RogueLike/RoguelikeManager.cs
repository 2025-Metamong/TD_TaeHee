using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using MyGame.Managers;

[RequireComponent(typeof(CanvasGroup))]
public class RoguelikeManager : MonoBehaviour
{
    public static RoguelikeManager Instance; // 다른 오브젝트들이 부르기 쉽게 싱글톤으로 설정.
    [Header("UI 참조")]
    public GameObject upgradeMenuPanel;      // 업그레이드 창 루트 (비활성화 상태로 에디터에 둠)
    public Button[] optionButtons = new Button[3];  // 3개의 선택 버튼
    public GameObject RLCard;   // 로그라이크 카드 Prefab

    [Header("업그레이드 데이터")]
    public List<RogueUpgrade> allUpgrades;    // ScriptableObject 등으로 정의한 업그레이드 목록

    private CanvasGroup _canvasGroup;

    // Monster 관리용 누적 데이터
    private float accDecreaseFlatHealth = 0;
    private float accDecreasePercentHealth = 1;
    private float accDecreaseFlatSpeed = 0;
    private float accDecreasePercentSpeed = 1;
    private int accExtraCoin = 0;

    void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        // _canvasGroup = GetComponent<CanvasGroup>();
        upgradeMenuPanel.SetActive(false);
    }

    public void ResetRoguelikeUpgrade()
    {
        this.accDecreaseFlatHealth = 0;
        this.accDecreasePercentHealth = 1;
        this.accDecreaseFlatSpeed = 0;
        this.accDecreasePercentSpeed = 1;
        this.accExtraCoin = 0;
        
        MonsterManager.Instance.ResetRoguelike();
        TowerManager.Instance.ResetRoguelike();
    }

    // 업그레이드 메뉴를 화면에 띄우고, 3가지 랜덤 옵션을 버튼에 세팅한다.
    public void ShowUpgradeMenu()
    {
        // 메뉴 보이기
        upgradeMenuPanel.SetActive(true);
        // _canvasGroup.interactable = true;
        // _canvasGroup.blocksRaycasts = true;

        // 랜덤으로 3개 고르기
        var choices = allUpgrades.OrderBy(u => UnityEngine.Random.value).Take(3).ToArray();

        // 태희 수정. 3개의 업그레이드 돌면서 RL카드 인스턴스화 하기.
        int i = 0;
        foreach (var upgrade in choices)
        {
            // 카드 인스턴스화 밑 정보 구현부
            upgrade.RandomizeValue();
            var instance = Instantiate(RLCard, upgradeMenuPanel.transform);
            var script = instance.GetComponent<MonoBehaviour>();
            var setData = script.GetType()?.GetMethod("SetRLCard", new Type[] { typeof(RogueUpgrade) });
            setData?.Invoke(script, new object[] { upgrade });
            var getButton = script.GetType()?.GetMethod("GetSelectButton", new Type[] { });
            var button = getButton?.Invoke(script, new object[] { });
            instance.SetActive(true);

            // optionButtons에 Select 버튼 매핑하기
            optionButtons[i] = (Button)button;
            optionButtons[i].onClick.RemoveAllListeners();
            optionButtons[i].onClick.AddListener(() => OnUpgradeSelected(upgrade));
            i++;

        }

        // 원래 태연이 코드.
        // for (int i = 0; i < optionButtons.Length; i++)
        // {
        //     var btn = optionButtons[i];
        //     choices[i].RandomizeValue();
        //     var data = choices[i];

        //     // 텍스트 세팅 (TextMeshProUGUI 쓰고 있다면 TMP 컴포넌트로 교체)
        //     btn.GetComponentInChildren<Text>().text = $"{data.upgradeName}\n\n{data.value:F2}";

        //     // 기존 리스너 제거 후 새로 추가
        //     btn.onClick.RemoveAllListeners();
        //     btn.onClick.AddListener(() => OnUpgradeSelected(data));

        //     btn.gameObject.SetActive(true);
        // }
    }

    private void OnUpgradeSelected(RogueUpgrade selected)
    {
        // 실제 업그레이드 적용 로직
        switch (selected.rogueID)
        {
            // decreaseHealthPercent
            case 0:
                this.decreaseHealthPercent(selected);
                break;

            // decreaseHealthValue
            case 1:
                this.decreaseHealthValue(selected);
                break;

            // decreaseSpeedPercent
            case 2:
                this.decreaseSpeedPercent(selected);
                break;

            // decreaseSpeedValue
            case 3:
                this.decreaseSpeedValue(selected);
                break;

            // extraCoin
            case 4:
                this.extraCoin(selected);
                break;

            // increaseAttackSpeed
            case 5:
                this.increaseAttackSpeed(selected);
                break;

            // increaseDamagePercent
            case 6:
                this.increaseDamagePercent(selected);
                break;

            // increaseDamageValue
            case 7:
                this.increaseDamageValue(selected);
                break;

            default:
                Debug.Log($"ID가 등록되지 않았거나 잘못된 ID 입니다. ID : {selected.rogueID}");
                break;
        }

        // 메뉴 숨기기
        upgradeMenuPanel.SetActive(false);
        // _canvasGroup.interactable = false;
        // _canvasGroup.blocksRaycasts = false;
    }

    private void decreaseHealthPercent(RogueUpgrade selected)
    {
        this.accDecreasePercentHealth *= (1 - selected.value);
        MonsterManager.Instance.SetHealthDecrease(this.accDecreasePercentHealth, this.accDecreaseFlatHealth);
    }

    private void decreaseHealthValue(RogueUpgrade selected)
    {
        this.accDecreaseFlatHealth += selected.value;
        MonsterManager.Instance.SetHealthDecrease(this.accDecreasePercentHealth, this.accDecreaseFlatHealth);
    }

    private void decreaseSpeedPercent(RogueUpgrade selected)
    {
        this.accDecreasePercentSpeed *= (1 - selected.value);
        MonsterManager.Instance.SetSpeedDecrease(this.accDecreasePercentSpeed, this.accDecreaseFlatSpeed);
    }

    private void decreaseSpeedValue(RogueUpgrade selected)
    {
        this.accDecreaseFlatSpeed += selected.value;
        MonsterManager.Instance.SetSpeedDecrease(this.accDecreasePercentSpeed, this.accDecreaseFlatSpeed);
    }

    private void extraCoin(RogueUpgrade selected)
    {
        MonsterManager.Instance.SetExtraCoin((int)selected.value);
    }

    public void increaseAttackSpeed(RogueUpgrade selected){
        TowerManager.Instance.SetAttackSpeedIncrease(1f + selected.value);
    }
    
    public void increaseDamagePercent(RogueUpgrade selected)
    {
        TowerManager.Instance.SetDamageIncrease(selected.value, 0);
    }
    
    public void increaseDamageValue(RogueUpgrade selected)
    {
        TowerManager.Instance.SetDamageIncrease(0, selected.value);
    }

}
