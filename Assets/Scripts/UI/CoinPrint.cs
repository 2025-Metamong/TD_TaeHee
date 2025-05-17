using MyGame.Managers;
using TMPro;
using UnityEngine;

public class CoinPrint : MonoBehaviour
{
    private TextMeshProUGUI coinText;

    private void Awake()
    {
        coinText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        // MonsterManager.coin 대신 ResourceManager에서 현재 코인 값을 가져와 표시
        coinText.text = ResourceManager.Instance.CurrentCoins.ToString();
    }
}
