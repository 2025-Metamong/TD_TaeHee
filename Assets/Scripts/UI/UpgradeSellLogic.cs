using System;
using System.Collections;
using MyGame.Managers;
using MyGame.Objects;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSellLogic : MonoBehaviour
{
    public Tower towerScript;
    public GameObject message;
    public GameObject modiButtons;

    public Button closeButton;
    public Button sellButton;
    public Button upgradeButton;
    private Coroutine failNoticeCoroutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 타워 스크립트 가져오기.
        this.towerScript = GetComponent<Tower>();
        if (this.towerScript == null)
        {
            Debug.Log("타워 조작 UI에 타워 스크립트 로드 실패");
        }

        // 버튼에 이벤트 추가.
        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(OnClickClosed);

        upgradeButton.onClick.RemoveAllListeners();
        upgradeButton.onClick.AddListener(OnClickUpgrade);

        sellButton.onClick.RemoveAllListeners();
        sellButton.onClick.AddListener(OnClickSell);
    }

    // 닫기 버튼 눌리면 호출.
    public void OnClickClosed()
    {
        // UI 끄기
        this.towerScript.towerSelectUI.SetActive(false);
    }

    // 팔기 버튼 눌리면 호출.
    public void OnClickSell()
    {
        TowerManager.Instance.SellTower(towerScript.GetID());
    }

    // 업그레이드 버튼 눌리면 호출.
    public void OnClickUpgrade()
    {
        bool isComplete = towerScript.UpgradeTower();
        if (isComplete == false)
        {
            if (this.failNoticeCoroutine != null)
            {
                StopCoroutine(failNoticeCoroutine);
            }
            failNoticeCoroutine = StartCoroutine(FailNoticeForSec(1));
            return;
        }
    }

    // 정해진 초 만큼 실패 메시지 띄우기.
    public IEnumerator FailNoticeForSec(int sec)
    {
        this.message.SetActive(true);
        yield return new WaitForSeconds(sec);
        this.message.SetActive(false);
        failNoticeCoroutine = null;
    }

    // 타워 업그레이드, 판매 UI 보이기.
    public void ShowUI()
    {
        modiButtons.GetComponent<ModiButtons>().SetData(towerScript.GetUpgradeCost(), towerScript.GetSellPrice());
        modiButtons.SetActive(true);
        message.SetActive(false);
    }

}
