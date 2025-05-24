using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RLCard : MonoBehaviour
{
    // 1. Select 버튼 클릭되면 RouguelikeUI 의 OnUpgradeSelected(RLData) 호출 
    //      -> 이거보다 Button SelectButton 을 리턴시키는 게 나을 듯.
    //      그래서 GetSelectButton() 메서드 만들었음.
    // 2. SetRLCard(RogueUpgrade data)
    //      RoguelikeUI 에서 RogueUpgrade 전달 받아서 RLCard 정보 세팅.
    [Header("로그라이크 업그레이드 표출 카드")]
    [SerializeField, Tooltip("로그라이크 UI 스크립트 가지는 페이지")] public MonoBehaviour RLScript;
    [SerializeField, Tooltip("로그라이크 업그레이드 SO 받는 필드.")] public RogueUpgrade RLData;
    [SerializeField, Tooltip("로그라이크 업그레이드 이름.")] public TextMeshProUGUI upgradeName;
    [SerializeField, Tooltip("로그라이크 업그레이드 설명.")] public TextMeshProUGUI description;
    [SerializeField, Tooltip("로그라이크 업그레이드 아이콘.")] public Image iconImage;
    [SerializeField, Tooltip("업그레이드 선택 버튼.")] public Button selectButton;

    // Select 버튼에 이벤트 추가. 다른 오브젝트가 버튼 가져간다면 삭제하는 것이 안전.
    void Start()
    {
        // selectButton.onClick.RemoveAllListeners();
        // selectButton.onClick.AddListener(OnClick_Select);
    }

    // RogueUpgrade 전달 받아서 자신의 TMP 필드 값 업데이트.
    public void SetRLCard(RogueUpgrade data)
    {
        if (data == null)
        {
            Debug.Log("data is null");
            return;
        }
        this.RLData = data; // SO 받기.
        if (RLData == null) {
            Debug.Log("RLData is null");
            return;
        }
        this.upgradeName.text = RLData.upgradeName;
        this.description.text = RLData.getDescription();
        // this.iconImage.sprite = RLData.icon.sprite;  // RogueUpgrade 에 icon 이미지 추가되면 사용할 듯.
    }

    // Select 버튼 클릭 되었을 때 수행. 다른 오브젝트가 Select 버튼 안 가져갔다는 가정하에 사용해야 할 듯.
    // public void OnClick_Select()
    // {
    //     if (RLScript == null)
    //     {
    //         Debug.Log("RoguelikeUI 스크립트 찾기 실패");
    //         return;
    //     }
    //     // 스크립트의 클릭 이벤트 호출...
    //     // var method = RLScript.GetType()?.GetMethod("OnUpgradeSelected", new Type[] { typeof(RogueUpgrade) });
    //     // method?.Invoke(RLScript, new object[] { this.RLData });
    // }

    // 다른 오브젝트가 RLCard의 셀렉트 버튼을 가져가고 싶을 때.
    public Button GetSelectButton()
    {
        return this.selectButton;
    }
    
}
