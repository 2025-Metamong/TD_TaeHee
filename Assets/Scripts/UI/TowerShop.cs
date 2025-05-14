using UnityEngine;

public class TowerShop : MonoBehaviour
{
    public GameObject panel;
    private int flag = 0;

    public void OnButtonClick()
    {
        if (flag == 0)
        {
            panel.SetActive(true); // UI ∫∏¿Ã±‚
        }
        else if (flag == 1)
        {
            panel.SetActive(false);
            flag = 0;
        }

    }
}
