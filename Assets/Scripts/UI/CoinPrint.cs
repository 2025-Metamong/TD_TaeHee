using MyGame.Managers;
using System.Xml;
using TMPro;
using UnityEngine;

public class CoinPrint : MonoBehaviour
{
    [SerializeField] private StageInfo stageInfo;
    public TextMeshProUGUI coinText;
    //public MonsterManager monsterManager;

    private void Start()
    {
        coinText = GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        //coinText.text = stageInfo.startCoins.ToString();
        coinText.text = StageManager.Instance.Coin.ToString();
    }
}
