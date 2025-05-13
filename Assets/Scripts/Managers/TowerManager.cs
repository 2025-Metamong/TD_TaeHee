using UnityEngine;
using MyGame.Objects;
using System.Collections.Generic;
using System;

namespace MyGame.Managers
{
    public class TowerManager : MonoBehaviour
    {
        // 현재 씬 안에 있는 타워들을 관리하는 타워 매니저 클래스. 싱글톤으로 객체 생성함.
        [Header("TowerManager Info")]
        [SerializeField] private GameObject towerPrefab;    // 생성할 타워의 prefab
        // private Tower[] towerList;  // TowerManager가 관리중인 타워들의 리스트.
        private List<GameObject>towerList = new List<GameObject>();  // 타워 리스트를 게임 오브젝트 리스트로 수정.
        private int TowerIndex = 0;
        public static TowerManager Instance { get; private set; } // 싱글톤 패턴
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            // 
        }

        // Update is called once per frame
        void Update()
        {

        }
        public void InstallTower(Vector3 position){  // 설치할 위치만 전달 받기로 결정.
        //    bool isEnough = ResourceManager.Instance.useCoins(T.GetCost());
           bool isEnough = true;
           // 돈이 충분한지 검사해서 타워를 설치하는 함수.
           if(isEnough != false){ // 타워 설치를 위한 돈이 충분했던 경우.
               // 사용된 Instantiate 함수 prototype : 
               //   public static Object Instantiate(Object original, Vector3 position, Quaternion rotation, Transform parent);
               GameObject newTower = Instantiate(towerPrefab, position, Quaternion.identity, this.transform);  // 새로운 타워 인스턴시에이트. 
               // 인스턴스에 사용할 Prefab towerPrefab. 위치는 전달 받은 Vector3 position. 회전은? 0. 부모는 TowerManager.

               if(newTower == null){
                   Debug.Log("타워 설치 실패");
                   return;
               }
               Debug.Log("타워 인스턴스화 성공");
               var newTowerScript = newTower?.GetComponent<MonoBehaviour>();    // 새 타워에서 타워 스크립트 찾기
               newTowerScript?.GetType()?.GetMethod("SetID", new Type[]{typeof(int)})?.Invoke(newTowerScript, new object[]{this.TowerIndex++});
               this.towerList.Add(newTower);   // 타워 List에 타워 Add
           }
           else {
               return;
           }
        }
        public void SellTower(GameObject toDelete){
            Debug.Log("Tower Selling");
        }
    
    
    }

}

