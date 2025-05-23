using System;
using System.Collections.Generic;
using MyGame.Objects;
using MyGame.Managers;
using UnityEngine;
using UnityEngine.UI;

public class TowerCard : MonoBehaviour
{
    [Header("타워 카드 정보")]
    [SerializeField, Tooltip("타워 프리팹")] private GameObject towerPrefab;
    [SerializeField, Tooltip("타워 정보 카드 프리팹")] private GameObject towerInfoPrefab;
    private GameObject towerInfoCard;   // 타워 인포 카드 인스턴스.
    [SerializeField, Tooltip("타워 이미지")] private Image towerImage;
    [SerializeField, Tooltip("타워 설치 버튼")] private Button installButton;
    [SerializeField, Tooltip("타워 정보 버튼")] private Button infoButton;
    [SerializeField, Tooltip("이미지 마스크")] private RectTransform maskTransform;
    [SerializeField, Tooltip("프리팹의 타워 스크립트")] private Tower towerScript;
    // private List<MonoBehaviour> childButtons = new List<MonoBehaviour>();
    void Start()
    {
        towerScript = towerPrefab.GetComponent<Tower>();
        // childButtons = new List<MonoBehaviour>(GetComponentsInChildren<MonoBehaviour>());
        // foreach (var button in childButtons)
        // {
        //     var setMethod = button.GetType()?.GetMethod("SetPrefab", new Type[] { typeof(GameObject) });
        //     setMethod?.Invoke(button, new object[] { towerPrefab });
        // }

        installButton.onClick.RemoveAllListeners();
        installButton.onClick.AddListener(ToInstallMode);
        infoButton.onClick.RemoveAllListeners();
        infoButton.onClick.AddListener(ToggleInfo);

        // 이미지 인스턴스 화.
        Instantiate(towerImage, this.maskTransform, false);

    }

    // 정보 버튼 클릭 시 실행.
    private void ToggleInfo()
    {
        Debug.Log("타워 정보 버튼 클릭.");
        if (towerInfoCard == null)
        {
            Debug.Log("타워 정보 카드 생성");
            // 1) 정보 카드 인스턴스화
            towerInfoCard = Instantiate(towerInfoPrefab, transform.parent, false);

            // 2) 데이터 전달
            towerInfoCard.GetComponent<TowerInfoCard>().SetData(towerPrefab);
            towerInfoCard.GetComponent<TowerInfoCard>().SetTowerCard(gameObject);

            // 3) 같은 형제 인덱스에 배치
            int idx = transform.GetSiblingIndex();
            towerInfoCard.transform.SetSiblingIndex(idx);
            towerInfoCard.SetActive(true);

            // 4) 카드 비활성화
            gameObject.SetActive(false);
            return;
        }
        else
        {
            Debug.Log("타워 정보 카드 토글");
            // 이미 생성된 경우에는 단순 토글
            bool isInfoActive = towerInfoCard.activeSelf;
            towerInfoCard.SetActive(!isInfoActive);
            gameObject.SetActive(isInfoActive);
            return;
        }
    }

    // 설치 버튼 클릭 시 실행.
    private void ToInstallMode()
    {
        Debug.Log("Install Button Clicked");
        bool buildMode = TowerManager.Instance.GetMode();
        if (!buildMode)
        {
            // Tower StageManager가 만들어지면 이 코드를 사용해 볼 예정.
            // if (StageManager.Instance.useCoins(this.towerScript.GetCost()))
            // {
            //     TowerManager.Instance.SetMode(true);
            //     TowerPlacementTile.towerPrefab = this.towerPrefab;
            // }
            // else
            // {
            //     Debug.Log($"타워{this.towerPrefab.name} 건축에 필요한 돈이 부족");
            //     return;
            // }

            // Tower StageManager가 만들어지면 이 코드는 사용 안할 것.
            TowerManager.Instance.SetMode(true);    // 건축 모드 활성화
            TowerPlacementTile.towerPrefab = this.towerPrefab;
        }
        Debug.Log($"설치 모드 : {TowerManager.Instance.GetMode()}");
    }

    public void SetTowerPrefab(GameObject tower)
    {
        this.towerPrefab = tower;
        // if (this.towerPrefab)
        // {
        //     Debug.Log("타워 카드에 프리팹 로드 성공");
        // }
        towerImage.sprite = this.towerPrefab.GetComponent<Tower>().portrait;
    }
}
