using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerShopPanel : MonoBehaviour
{
    [Header("Tower Shop Panel")]
    [SerializeField, Tooltip("닫기 버튼")] public Button closeButton;
    [SerializeField, Tooltip("타워 샵 버튼")] public GameObject towerShopButton;
    [SerializeField, Tooltip("패널이 표시할 오브젝트 리스트")] public List<GameObject> TowerCards = new List<GameObject>();

    void Start()
    {
        closeButton = GetComponentInChildren<Button>(); // 닫기 버튼 찾기.
        closeButton.onClick.AddListener(ClosePanel);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void ClosePanel()
    {
        this.gameObject.SetActive(false);
        this.towerShopButton.SetActive(true);
    }
}
