using MyGame.Objects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerInfoButton : MonoBehaviour
{
    public TextMeshProUGUI Text;    // 표기 텍스트
    public GameObject towerPrefab;
    private Tower towerScript;
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(ToInstallMode);
        Text = GetComponentInChildren<TextMeshProUGUI>();
        Text.text = "info";

        towerScript = towerPrefab.GetComponent<Tower>();
        if (towerScript != null)
        {
            Debug.Log("타워 스크립트 로드 성공");
        }
    }

    private void ToInstallMode()
    {
        Debug.Log("Info Button Clicked");
        // This.ShowTowerInfo(Tower.GetTowerInfo());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
