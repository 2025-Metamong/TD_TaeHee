using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using MyGame.Objects;

public class TowerInfoCard : MonoBehaviour
{
    // 타워 정보 받아서 출력하는 카드. Tower Prefab으로부터 정보 가져옴.
    [Header("타워 정보 카드")]
    private GameObject towerCard;
    [SerializeField, Tooltip("닫기 버튼")] private Button closeButton;
    private GameObject tower;
    private Tower towerScript;
    [SerializeField, Tooltip("디버프 아이콘 리스트")] private List<Image> debuffIcons = new List<Image>();
    [SerializeField, Tooltip("탄환 아이콘 리스트")] private Image bulletIcons;
    [SerializeField, Tooltip("타워 공격력")] private TextMeshProUGUI damage;
    [SerializeField, Tooltip("타워 사거리")] private TextMeshProUGUI range;

    [SerializeField, Tooltip("타워 가격")] private TextMeshProUGUI cost;

    void Start()
    {
        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(ToggleTowerCard);
    }

    private void ToggleTowerCard()
    {
        if (towerCard == null)
        {
            Debug.Log("정보 카드에 타워 카드 로드 안됨.");
        }

        int idx = transform.GetSiblingIndex();
        towerCard.transform.SetSiblingIndex(idx);

        towerCard.SetActive(true);
        gameObject.SetActive(false);
    }

    // 토글할 타워 카드.
    public void SetTowerCard(GameObject tCard)
    {
        this.towerCard = tCard;
    }
    // Tower Prefab을 전달 받아 관련 데이터를 설정하기.
    public void SetData(GameObject towerObj)
    {
        this.tower = towerObj;
        this.towerScript = towerObj.GetComponent<Tower>();

        if (tower == null)
        {
            Debug.Log("타워 정보 카드에 타워 스크립트 로딩 실패");
            return;
        }
        // 타워 정보 카드 값 업데이트.
        this.damage.text = towerScript.GetDamage().ToString();
        this.range.text = towerScript.GetRange().ToString();
        this.cost.text = towerScript.GetCost().ToString();

        // 이 아래는 테스트용. SO 에서 아이콘을 관리하게 되면 수정할 것.
        // this.debuffIcons = towerScript.GetDebuffIcons();
        // this.bulletIcons = towerScript.GetbulletIcon();

        // var debuffs = towerScript.GetDebuffList();
    }


}
