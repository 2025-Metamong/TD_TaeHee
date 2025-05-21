using MyGame.Managers;
using System.Xml;
using TMPro;
using UnityEngine;

public class HpPrint : MonoBehaviour
{
    [SerializeField] private StageInfo stageInfo;
    public TextMeshProUGUI hpText;

    private void Start()
    {
        hpText = GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        hpText.text = stageInfo.playerHP.ToString();
    }
}
