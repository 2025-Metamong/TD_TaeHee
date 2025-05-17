using MyGame.Managers;
using System.Xml;
using TMPro;
using UnityEngine;

public class HpPrint : MonoBehaviour
{
    public TextMeshProUGUI hpText;

    private void Start()
    {
        hpText = GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        hpText.text = MonsterManager.Hp.ToString();
    }
}
