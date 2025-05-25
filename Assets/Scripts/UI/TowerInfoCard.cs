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
    [SerializeField, Tooltip("디버프 프레임")] private GameObject debuffFrame;
    private GameObject tower;
    private Tower towerScript;
    [SerializeField, Tooltip("디버프 리스트")] private List<debuffBase> debuffList = new List<debuffBase>();
    [SerializeField, Tooltip("디버프 이미지 프리팹")] private Image debuffImage;
    private List<Image> liveDebuffImages = new List<Image>();   // 현재 있는 디버프 이미지들.
    [SerializeField, Tooltip("생성된 디버프 아이콘들")] private List<Sprite> debuffIcons = new List<Sprite>();
    [SerializeField, Tooltip("디버프 없는 경우 아이콘")] private Sprite defaultIcon;
    
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
        // 디버프 이미지 인스턴스들 죽이기.
        foreach (var liveImg in liveDebuffImages)
        {
            Destroy(liveImg);
        }
        liveDebuffImages.Clear();
        debuffIcons.Clear();

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

        // 타워의 디버프 리스트 가져오기.
        this.debuffList = towerScript.GetDebuffAssets();

        if (debuffList == null)
        {
            Debug.Log("타워의 디버프들 로딩 실패");
        }

        // 아이콘 추출하기
        foreach (var dbuf in debuffList)
        {
            Debug.Log("아이콘 추가?");
            debuffIcons.Add(dbuf.Icon);
        }

        // 추출 된 아이콘 없으면 기본 아이콘으로.
        if (debuffIcons.Count <= 0)
        {
            // Debug.Log("기본 아아이콘 생성");
            this.debuffIcons.Add(defaultIcon);
            Image img = Instantiate(debuffImage, debuffFrame.GetComponent<RectTransform>());
            img.sprite = defaultIcon;
            liveDebuffImages.Add(img);
            return;
        }

        // 디버프 이미지들 그리기.
        foreach (var icon in debuffIcons)
        {
            // Debug.Log("디버프 아이콘 생성");
            Image img = Instantiate(debuffImage, debuffFrame.GetComponent<RectTransform>());
            img.sprite = icon;
            liveDebuffImages.Add(img);
        }

        // var debuffs = towerScript.GetDebuffList();
    }


}
