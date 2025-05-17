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
    void TowerShopClicked()
    {
        panel.SetActive(true); 
        this.gameObject.SetActive(false);
    }
}
