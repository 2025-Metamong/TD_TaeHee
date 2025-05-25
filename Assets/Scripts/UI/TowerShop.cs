using UnityEngine;
using UnityEngine.UI;

public class TowerShop : MonoBehaviour
{
    public GameObject panel;
    private Button towerShopBtn;
    void Start()
    {
        towerShopBtn = GetComponent<Button>();
        towerShopBtn.onClick.AddListener(TowerShopClicked);
    }

    void Update()
    {
        // 로그라이크 매니저에서 업그레이드 패널이 액티브면
        if (RoguelikeManager.Instance.upgradeMenuPanel.activeSelf == true)
        {
            gameObject.SetActive(false); // 자기자신 숨기기
        }
    }
    void TowerShopClicked()
    {
        panel.SetActive(true); 
        this.gameObject.SetActive(false);
    }
}
