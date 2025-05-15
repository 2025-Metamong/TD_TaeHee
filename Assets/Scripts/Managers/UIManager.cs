using MyGame.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //public static UIManager Instance { get; private set; }

    //[Header("Resource UI")]
    //[SerializeField] private TMP_Text hpText;
    //[SerializeField] private TMP_Text coinText;

    //[Header("Wave UI")]
    //[SerializeField] private TMP_Text waveText;
    //[SerializeField] private Button waveStartButton;

    //[Header("Tower Select UI")]
    //[SerializeField] private GameObject towerSelectPanel;

    //private void Awake()
    //{
    //    if (Instance == null) Instance = this;
    //    else Destroy(gameObject);
    //}

    //private void Start()
    //{
    //    waveStartButton.onClick.AddListener(onClickWaveStart);
    //}

    //public void UpdateHPUI(int hp)
    //{
    //    // HP 텍스트 업데이트
    //    hpText.text = $"HP: {hp}";
    //}

    //public void UpdateCoinUI(int coin)
    //{
    //    // Coin 텍스트 업데이트
    //    coinText.text = $"Coin: {coin}";
    //}

    //public void UpdateWaveUI(int wave, int totalWaves)
    //{
    //    // Wave 텍스트 업데이트
    //    waveText.text = $"Wave {wave + 1} / {totalWaves}";
    //}

    //public void ShowTowerSelectPanel()
    //{
    //    // 타워 선택창 열기
    //    towerSelectPanel.SetActive(true);
    //}

    //public void HideTowerSelectPanel()
    //{
    //    // 타워 선택창 닫기
    //    towerSelectPanel.SetActive(false);
    //}

    //public void OnClickWaveStart()
    //{
    //    // Wave 시작 버튼 활성화
    //    waveStartButton.interactable = true;
    //}

    //public void DisableClickWaveStart()
    //{
    //    // Wave 시작 버튼 비활성화
    //    waveStartButton.interactable = false;
    //}


    //승원's code TowerShop에 대한 코드인데 현재 TowerShop 스크립트에서 관리하기로 함

    //public Button towerShop;
    //public GameObject panel;
    //public TowerManager towerManager;
    //public GameObject itemTemplate;
    //public Transform contentParent;

    //private int flag = 0;
    //private bool panelOpen = false;
    //void Start()
    //{
    //    //towerShop.onClick.AddListener(PopulateShop);
    //    towerShop.onClick.AddListener(TowerShopClicked);
    //    panel.SetActive(false); // 시작 시 비활성화

    //    // 💡 Start에서 미리 로딩
    //    PopulateShop();
    //}
    //void TowerShopClicked()
    //{
    //    if (flag == 0)
    //    {
    //        panel.SetActive(true); // UI 보이기
    //        flag = 1;
    //    }
    //    else if (flag == 1)
    //    {
    //        panel.SetActive(false);
    //        flag = 0;
    //    }
    //}

    //void TowerShopClicked()
    //{
    //    if (flag == 0)
    //    {
    //        panel.SetActive(true); // UI 보이기
    //        flag = 1;
    //    }
    //    else if (flag == 1)
    //    {
    //        panel.SetActive(false);
    //        flag = 0;
    //    }
    //}

    //void PopulateShop()
    //{
    //    foreach (var prefab in towerManager.prefabList)
    //    {
    //        GameObject newItem = Instantiate(itemTemplate, contentParent);
    //        RenderTexture renderTexture = new RenderTexture(256, 256, 16);
    //        renderTexture.Create();

    //        Camera camera = new GameObject("PrefabCamera").AddComponent<Camera>();
    //        camera.targetTexture = renderTexture;
    //        camera.transform.position = prefab.transform.position + new Vector3(0, 1, -2);
    //        camera.transform.LookAt(prefab.transform);

    //        RawImage rawImage = newItem.GetComponentInChildren<RawImage>();
    //        rawImage.texture = renderTexture;

    //        // 이름 설정
    //        Text nameText = newItem.GetComponentInChildren<Text>();
    //        nameText.text = prefab.name;
    //    }
    //}

}
