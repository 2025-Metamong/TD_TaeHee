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
        [SerializeField, Tooltip("타워 프리팹 리스트")] private List<GameObject> towerPrefabs;
        [SerializeField, Tooltip("글로벌 공격력 증가 팩터 N")] private float nDamage;
        [SerializeField, Tooltip("글로벌 공격력 증가 팩터 M")] private float mDamage;
        [SerializeField, Tooltip("글로벌 공격속도 증가 팩터 Speed")] private float globalSpeedModifier;
        private Dictionary<int, GameObject> towerDict = new Dictionary<int, GameObject>();  // 현재 소환 된 타워 리스트를 딕셔너리로 수정

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
        public void InstallTower(Vector3 position)
        {  // 설치할 위치만 전달 받기로 결정.
           //    bool isEnough = ResourceManager.Instance.useCoins(T.GetCost());
            bool isEnough = true;
            // 돈이 충분한지 검사해서 타워를 설치하는 함수.
            if (isEnough != false)
            { // 타워 설치를 위한 돈이 충분했던 경우.
              // 사용된 Instantiate 함수 prototype : 
              //   public static Object Instantiate(Object original, Vector3 position, Quaternion rotation, Transform parent);
                GameObject newTower = Instantiate(towerPrefabs[0], position, Quaternion.identity, this.transform);  // 새로운 타워 인스턴시에이트. 

                if (newTower == null)
                {
                    Debug.Log("타워 설치 실패");
                    return;
                }
                Debug.Log("타워 인스턴스화 성공");
                if (!newTower.TryGetComponent<MonoBehaviour>(out var newTowerScript))   // 새 타워에서 스크립트 찾기.
                {
                    Debug.Log("객체에 스크립트 존재하지 않음");
                    return;
                }
                // 타워 ID 세팅
                newTowerScript.GetType()?.GetMethod("SetID", new Type[] { typeof(int) })?.Invoke(newTowerScript, new object[] { this.TowerIndex });
            
                // 글로벌 데미지 계산식 적용
                var originalDamage = newTowerScript.GetType()?.GetMethod("GetDamage", new Type[] {})?.Invoke(newTowerScript, new object[] { });
                float newDamage = (float)originalDamage * (1 + this.nDamage) + this.mDamage;
                newTowerScript.GetType()?.GetMethod("SetDamage", new Type[] {typeof(float)})?.Invoke(newTowerScript, new object[] { newDamage });

                // 글로벌 공격속도 계산식 적용

                this.towerDict.Add(this.TowerIndex++, newTower);   // 타워 List 딕셔너리리에 타워 Add
            }
            else
            {
                return;
            }
        }
        public void SellTower(GameObject toDelete)
        {
            Debug.Log("Tower Selling");
        }

        public IEnumerable<GameObject> GetTowerPrefabs()
        {
            return this.towerPrefabs;
        }
    
        // 현존하는 타워 + 이후 등장할 타워들 데미지 증가 기능
        public void SetDamageIncrease(float N, float M)
        {
            Debug.Log($"모든 타워 공격력 * (1 + {N}) + {M} 수행");

            this.nDamage = N;
            this.mDamage = M;

            foreach (var tower in this.towerDict)
            {
                if (!tower.Value.TryGetComponent<MonoBehaviour>(out var towerScript)) // 스크립트 찾기.
                {
                    Debug.Log("객체에 스크립트 존재하지 않음");
                    continue;
                }
                // 존재하는 타워 중 하나 데미지 증가 수행.
                var originalDamage = towerScript.GetType()?.GetMethod("GetDamage", new Type[] {})?.Invoke(towerScript, new object[] { });
                float newDamage = (float)originalDamage * (1 + N) + M;
                var damageIncMethod = towerScript.GetType()?.GetMethod("SetDamage", new Type[] { typeof(float) });
                damageIncMethod?.Invoke(towerScript, new object[] { newDamage });
            }

        }

        // 현존하는 타워 + 이후에 등장하는 타워 모두 데미지 증가 기능.
        public void SetAttackSpeedIncrease(float N)
        {
            Debug.Log($"모든 타워 공격 속도 * {N} 수행");

            this.globalSpeedModifier = N;

            foreach (var tower in this.towerDict)
            {
                if (!tower.Value.TryGetComponent<MonoBehaviour>(out var towerScript)) // 스크립트 찾기.
                {
                    Debug.Log("객체에 스크립트 존재하지 않음");
                    continue;
                }
                // 존재하는 타워들 하나씩 공격 속도 증가 수행.
                var originalAttackPeriod = towerScript.GetType()?.GetMethod("GetAttakPeriod", new Type[] {})?.Invoke(towerScript, new object[] { });
                float newAttackPeriod = (float)originalAttackPeriod * N;
                var attackSpeedSetMethod = towerScript.GetType()?.GetMethod("SetAttakPeriod", new Type[] { typeof(float) });
                attackSpeedSetMethod?.Invoke(towerScript, new object[] { newAttackPeriod });
            }
        }
    
    }

}

