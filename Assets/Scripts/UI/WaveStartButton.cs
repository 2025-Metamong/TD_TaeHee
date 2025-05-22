using UnityEngine;
using UnityEngine.UI;

public class WaveStartButton : MonoBehaviour
{
    public StageManager targetScript;
    private Button waveStarButton;
    void Start()
    {
        waveStarButton = GetComponent<Button>();
        waveStarButton.onClick.AddListener(OnButtonClick);
    }
    public void OnButtonClick()
    {
        targetScript.SetFlag(true);
    }
}
