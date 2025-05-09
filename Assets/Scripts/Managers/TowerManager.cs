using UnityEngine;
using MyGame.Objects;

namespace MyGame.Managers
{
    public class TowerManager : MonoBehaviour
    {
        // 현재 씬 안에 있는 타워들을 관리하는 타워 매니저 클래스. 싱글톤으로 객체 생성함.
        [Header("TowerManager Info")]
        [SerializeField] private GameObject towerPrefab;    // 생성할 타워의 prefab
        private Tower[] towerList;  // TowerManager가 관리중인 타워들의 리스트.
        public static TowerManager Instance { get; private set; } // 싱글톤 
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
            // 기본 설치된 타워들이 있는지 확인하고 가지고 옴. 스크립트 컴포넌트만 가지고 온다.
            this.towerList = gameObject.GetComponentsInChildren<Tower>();
        }

        // Update is called once per frame
        void Update()
        {

        }
        //public void InstallTower(Tower T){  // 여기 굳이 타워를 전달 받아야하나 의문. 그냥 설치할 위치만 전달 받아도 될 것 같은데
        //    // 돈이 충분한지 검사해서 타워를 설치하는 함수.
        //    if(ResourceManager.Instance.useCoins(T.GetCost()) != false){ // 타워 설치를 위한 돈이 충분했던 경우.
        //        // 사용된 Instantiate 함수 prototype : 
        //        //      public static Object Instantiate(Object original, Vector3 position, Quaternion rotation, Transform parent);
        //        GameObject newTower = Instantiate(towerPrefab, T.transform.position, Quaternion.identity, this.transform);  // 새로운 타워 인스턴시에이트. 
        //        // 인스턴스에 사용할 Prefab towerPrefab. 위치는 전달 받은 타워의 position. 회전은? 0. 부모는 TowerManager.

        //        if(newTower == null){
        //            Debug.Log("타워 설치 실패");
        //            return;
        //        }
        //        Debug.Log("타워 설치 성공");
        //        this.towerList = gameObject.GetComponentsInChildren<Tower>();   // 타워 스크립트들 찾아오기.
        //        // 큐나 리스트 같은 걸로 관리해도 되는데 귀찮아서...
        //    }
        //    else {
        //        return;
        //    }

        //}
    }

}

