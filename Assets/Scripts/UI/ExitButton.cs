using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ExitButton : MonoBehaviour
{
    private Button _button;
    void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClicked);
    }

    void OnDestroy()
    {
        _button.onClick.RemoveListener(OnClicked);
    }

    private void OnClicked()
    {
        StageManager.Instance.FinishStage();
        GameManager.Instance.ExitScene();
    }
}
