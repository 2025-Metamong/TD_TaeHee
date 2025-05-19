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
        towerScript = towerPrefab.GetComponent<Tower>();
    }

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

    // Update is called once per frame
    void Update()
    {

    }

    public void SetPrefab(GameObject tower)
    {
        this.towerPrefab = tower;
        Debug.Log($"tower prefab name {towerPrefab.name}");
    }
}
