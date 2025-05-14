using MyGame.Managers;
using System.Xml;
using TMPro;
using UnityEngine;

public class CoinPrint : MonoBehaviour
{
    public TextMeshProUGUI coinText;
    //public MonsterManager monsterManager;

    private void Start()
    {
        coinText = GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        coinText.text = MonsterManager.coin.ToString();  
    }
}
