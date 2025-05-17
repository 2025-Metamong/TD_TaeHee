using MyGame.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InstallButton : MonoBehaviour
{
    public TextMeshProUGUI Text;
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
