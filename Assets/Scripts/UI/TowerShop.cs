using UnityEngine;
using UnityEngine.UI;

public class TowerShop : MonoBehaviour
{
    // 현재 사용안함

    public GameObject panel;

    private int flag = 0;
    private bool panelOpen = false;
    private Button towerShopBtn;
    void Start()
    {
        towerShopBtn = GetComponent<Button>();
        towerShopBtn.onClick.AddListener(TowerShopClicked);
    }
    void TowerShopClicked()
    {
        if (flag == 0)
        {
            panel.SetActive(true); // UI 보이기
            flag = 1;
            this.gameObject.SetActive(false);
        }
        else if (flag == 1)
        {
            panel.SetActive(false);
            flag = 0;
        }
    }
}
