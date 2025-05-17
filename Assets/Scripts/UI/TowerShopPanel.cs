using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerShopPanel : MonoBehaviour
{
    [Header("Tower Shop Panel")]
    [SerializeField, Tooltip("닫기 버튼")] public Button closeButton;
    [SerializeField, Tooltip("패널이 표시할 오브젝트 리스트")] public List<GameObject> TowerCards = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
