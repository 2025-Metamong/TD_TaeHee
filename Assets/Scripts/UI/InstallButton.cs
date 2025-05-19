using MyGame.Managers;
using MyGame.Objects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InstallButton : MonoBehaviour
{
    public TextMeshProUGUI Text;
    public GameObject towerPrefab;
    private Tower towerScript;
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(ToInstallMode);
        Text = GetComponentInChildren<TextMeshProUGUI>();
        Text.text = "install";
    }

    private void ToInstallMode()
    {
        Debug.Log("Install Button Clicked");
        // TowerManager.Instance.InstallMode();
        bool buildMode = TowerManager.Instance.GetMode();
        if (!buildMode)
        {
            TowerManager.Instance.SetMode(true);
        }
        Debug.Log($"설치 모드 : {TowerManager.Instance.GetMode()}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
