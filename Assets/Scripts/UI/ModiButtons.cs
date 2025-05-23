using UnityEngine;
using TMPro;

public class ModiButtons : MonoBehaviour
{
    public TextMeshProUGUI upgradeText;
    public TextMeshProUGUI sellText;    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    public void SetData(int upCost, int sellP)
    {
        this.upgradeText.text = "UPGRADE\n$" + upCost;
        this.sellText.text = "SELL\n$" + sellP;
    }
}
