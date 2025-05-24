using System.Collections.Generic;
using MyGame.Managers;
using UnityEngine;
using UnityEngine.UI;

public class TowerShopPanel : MonoBehaviour
{
    [Header("Tower Shop Panel")]
    [SerializeField, Tooltip("닫기 버튼")] public Button closeButton;
    [SerializeField, Tooltip("타워 샵 버튼")] public GameObject towerShopButton;
    [SerializeField, Tooltip("타워 카드 프리팹")] public GameObject towerCard;
    [SerializeField, Tooltip("타워 카드 프리팹")] public GameObject towerInfoCard;
    [SerializeField, Tooltip("카드들의 부모")] public Transform parentTransform;

    private List<GameObject> towerCardList = new List<GameObject>();    // 타워 카드 인스턴스 관리
    private List<GameObject> towerInfoList = new List<GameObject>();    // 타워 정보 카드 인스턴스 관리
    void Start()
    {
        parentTransform = this.transform;
        closeButton = GetComponentInChildren<Button>(); // 닫기 버튼 찾기.
        closeButton.onClick.AddListener(ClosePanel);

        ShowTowerCards();

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void ShowTowerCards()
    {
        var towerList = TowerManager.Instance.GetTowerPrefabs();
        foreach (var towerData in towerList)
        {
            Debug.Log("타워 카드 인스턴스 화");
            GameObject card = Instantiate(towerCard, parentTransform);
            if (card == null)
            {
                Debug.Log("타워 카드 인스턴스 화 실패");
            }
            // if (towerData)
            // {
            //     Debug.Log("타워 샵 패널에서 타워 프리팹 로드 성공");
            // }

            // 카드에 프리팹 전달
            card.GetComponent<TowerCard>().SetTowerPrefab(towerData);
            
            // 카드 활성화화
            card.SetActive(true);
        }
    }

    // 패널 닫기 함수.
    private void ClosePanel()
    {
        this.gameObject.SetActive(false);
        this.towerShopButton.SetActive(true);
    }
}
