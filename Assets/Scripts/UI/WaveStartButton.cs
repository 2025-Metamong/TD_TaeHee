using UnityEngine;
using UnityEngine.UI;

public class WaveStartButton : MonoBehaviour
{
    private Button waveStarButton;
    void Start()
    {
        waveStarButton = GetComponent<Button>();
        waveStarButton.onClick.AddListener(OnButtonClick);
    }
    public void OnButtonClick()
    {
        Debug.Log("Wave Start Button Clicked");
        StageManager.Instance.SetFlag(true);
    }
}
