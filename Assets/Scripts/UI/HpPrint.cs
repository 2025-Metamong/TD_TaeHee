using MyGame.Managers;
using TMPro;
using UnityEngine;

public class HpPrint : MonoBehaviour
{
    private TextMeshProUGUI hpText;

    private void Awake()
    {
        hpText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        // ResourceManager에서 현재 라이프(HP) 값을 가져와 표시
        hpText.text = ResourceManager.Instance.CurrentLives.ToString();
    }
}
