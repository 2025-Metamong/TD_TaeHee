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
        [SerializeField, Tooltip("설치 모드인지 아닌지 표기")] private bool mode = false; // 타워 매니저에서 관리하는 게 나을까?
        [SerializeField, Tooltip("글로벌 공격력 증가 팩터 N")] private float nDamage = 1f;
        [SerializeField, Tooltip("글로벌 공격력 증가 팩터 M")] private float mDamage = 0f;
        [SerializeField, Tooltip("글로벌 공격속도 증가 팩터 Speed")] private float globalSpeedModifier = 1f;
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
        // 설치한 타워의 ID 리턴하기로 변경. -> 타일의 초기화 위해.
        public int InstallTower(GameObject tower, Vector3 position)
        {  // 설치할 타워까지 전달 받기로 변경.
            bool isEnough = StageManager.Instance.UseCoin(tower.GetComponent<Tower>().GetCost());
            //bool isEnough = true;
            // 돈이 충분한지 검사해서 타워를 설치하는 함수.
            if (isEnough != false)
            { // 타워 설치를 위한 돈이 충분했던 경우.
              // 사용된 Instantiate 함수 prototype : 
              //   public static Object Instantiate(Object original, Vector3 position, Quaternion rotation, Transform parent);
                GameObject newTower = Instantiate(tower, position, Quaternion.identity, this.transform);  // 새로운 타워 인스턴시에이트. 

                if (newTower == null)
                {
                    Debug.Log("타워 설치 실패");
                    // 설치 실패 사운드 재생
                    TowerSoundController.Instance.PlayFailedSound();
                    return -1;
                }
                Debug.Log("타워 인스턴스화 성공");
                if (!newTower.TryGetComponent<MonoBehaviour>(out var newTowerScript))   // 새 타워에서 스크립트 찾기.
                {
                    Debug.Log("객체에 스크립트 존재하지 않음");
                    TowerSoundController.Instance.PlayFailedSound();    // 실패 사운드 재생.
                    return -1;
                }
                // 타워 ID 세팅
                newTowerScript.GetType()?.GetMethod("SetID", new Type[] { typeof(int) })?.Invoke(newTowerScript, new object[] { this.TowerIndex });

                // 글로벌 데미지 계산식 적용
                var originalDamage = newTowerScript.GetType()?.GetMethod("GetDamage", new Type[] { })?.Invoke(newTowerScript, new object[] { });
                float newDamage = (float)originalDamage * this.nDamage + this.mDamage;
                newTowerScript.GetType()?.GetMethod("SetDamage", new Type[] { typeof(float) })?.Invoke(newTowerScript, new object[] { newDamage });

                // 글로벌 공격속도 계산식 적용

                this.towerDict.Add(this.TowerIndex++, newTower);   // 타워 List 딕셔너리리에 타워 Add

                // 타워 설치 성공 사운드 재생
                TowerSoundController.Instance.PlayInstallSound();
                return newTower.GetComponent<Tower>().GetID();  // 설치한 타워 ID 리턴.
            }
            else
            {
                TowerSoundController.Instance.PlayFailedSound();
                return -1;
            }
        }

        // 판매할 타워 아이디 전달 받아서 판매가 StageManager에 주고 Destroy하고 TowerDict에서 제거.
        public void SellTower(int toDeleteID)
        {
            Debug.Log("Tower Selling");
            GameObject toDelete = towerDict[toDeleteID];
            Tower script = toDelete.GetComponent<Tower>();
            StageManager.Instance.AddCoins(script.GetSellPrice());    // 구현되면 처리해야 함.
            towerDict.Remove(toDeleteID);
            Destroy(toDelete);
            // 타워 판매 사운드 재생
            TowerSoundController.Instance.PlaySellSound();
        }

        // 디버프 종류를 받아서 현재 존재하는 타워들에 추가하기. 추가할 종류가 여러개면 바깥에서 여러번 부르면 된다.
        public void AddDebuffToAllTower(debuffBase dbuff)
        {
            // 현재 인스턴스화 된 타워 순환
            foreach (var entry in towerDict)
            {
                entry.Value.GetComponent<Tower>().AddDebuff(dbuff); // 타워에 디버프 전달.
            }
        }
        public IEnumerable<GameObject> GetTowerPrefabs()
        {
            return this.towerPrefabs;
        }

        // 현존하는 타워 공격력 증가 + 추후 설치될 타워에 적용 위해 공격력 증가 계수 저장
        public void SetDamageIncrease(float N, float M)
        {
            Debug.Log($"모든 타워 공격력 * (1 + {N}) + {M} 수행");

            this.nDamage *= (1+N);
            this.mDamage += M;

            foreach (var tower in this.towerDict)
            {
                if (!tower.Value.TryGetComponent<MonoBehaviour>(out var towerScript)) // 스크립트 찾기.
                {
                    Debug.Log("객체에 스크립트 존재하지 않음");
                    continue;
                }
                // 존재하는 타워 중 하나 데미지 증가 수행.
                var originalDamage = towerScript.GetType()?.GetMethod("GetDamage", new Type[] { })?.Invoke(towerScript, new object[] { });
                float newDamage = (float)originalDamage * (1 + N) + M;
                var damageIncMethod = towerScript.GetType()?.GetMethod("SetDamage", new Type[] { typeof(float) });
                damageIncMethod?.Invoke(towerScript, new object[] { newDamage });
            }

        }

        // 현존하는 타워 공격 속도 증가 + 추후 설치될 타워에 적용 위해 공격 속도 증가 계수 저장.
        public void SetAttackSpeedIncrease(float N)
        {
            Debug.Log($"모든 타워 공격 속도 * {N} 수행");

            this.globalSpeedModifier *= N;

            foreach (var tower in this.towerDict)
            {
                if (!tower.Value.TryGetComponent<MonoBehaviour>(out var towerScript)) // 스크립트 찾기.
                {
                    Debug.Log("객체에 스크립트 존재하지 않음");
                    continue;
                }
                // 존재하는 타워들 하나씩 공격 속도 증가 수행.
                var originalAttackPeriod = towerScript.GetType()?.GetMethod("GetAttackPeriod", new Type[] { })?.Invoke(towerScript, new object[] { });
                float newAttackPeriod = (float)originalAttackPeriod * N;
                var attackSpeedSetMethod = towerScript.GetType()?.GetMethod("SetAttackPeriod", new Type[] { typeof(float) });
                attackSpeedSetMethod?.Invoke(towerScript, new object[] { newAttackPeriod });
            }
        }

        // 타워 딕셔너리 반환.
        public Dictionary<int, GameObject> GetTowerDict()
        {
            return this.towerDict;
        }

        public void ResetRoguelike()
        {
            this.nDamage = 1f;
            this.mDamage = 0f;

            this.globalSpeedModifier = 1f;
        }

        public bool GetMode()
        {
            return this.mode;
        }

        public void SetMode(bool newMode)
        {
            this.mode = newMode;
        }

    }
}