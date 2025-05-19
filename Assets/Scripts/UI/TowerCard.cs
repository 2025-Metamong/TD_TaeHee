using System;
using System.Collections.Generic;
using MyGame.Objects;
using UnityEngine;
using UnityEngine.UI;

public class TowerCard : MonoBehaviour
{
    [Header("타워 카드 정보")]
    [SerializeField, Tooltip("타워 프리팹")] private GameObject towerPrefab;
    [SerializeField, Tooltip("타워 이미지")] private Image towerImage;
    [SerializeField, Tooltip("프리팹의 타워 스크립트")] private Tower towerScript;
    private List<MonoBehaviour> childButtons = new List<MonoBehaviour>();
    void Start()
    {
        towerScript = towerPrefab.GetComponent<Tower>();
        childButtons = new List<MonoBehaviour>(GetComponentsInChildren<MonoBehaviour>());
        foreach (var button in childButtons)
        {
            var setMethod = button.GetType()?.GetMethod("SetPrefab", new Type[] { typeof(GameObject) });
            setMethod?.Invoke(button, new object[] { towerPrefab });
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetTowerPrefab(GameObject tower)
    {
        this.towerPrefab = tower;
    }
}
