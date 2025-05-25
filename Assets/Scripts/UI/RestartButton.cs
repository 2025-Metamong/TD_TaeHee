using MyGame.Managers;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace MyGame.UI
{
    public class RestartButton : MonoBehaviour
    {
        public static RestartButton Instance { get; private set; }

        private Button _button;
        int currentStage;
        void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClicked);
        }

        void OnEnable()
        {
            currentStage = StageManager.Instance.getCurrentStage;
        }

        private void OnClicked()
        {
            StageManager.Instance.ExitStage();
            GameManager.Instance.LoadScene("SelectStage");
            GameManager.Instance.LoadScene("InStage");
            StageManager.Instance.LoadStage(currentStage);
        }


    }
}