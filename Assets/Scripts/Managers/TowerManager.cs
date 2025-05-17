using UnityEngine;
using MyGame.Objects;
using System.Collections.Generic;

namespace MyGame.Managers
{
    public class TowerManager : MonoBehaviour
    {
        [Header("TowerManager Info")]
        [SerializeField] private GameObject towerPrefab;

        // 설치된 타워를 ID → GameObject로 관리
        private Dictionary<int, GameObject> towerDict = new Dictionary<int, GameObject>();

        public static TowerManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }

        }

        /// <summary>
        /// 화면 상의 월드 좌표로 타워 설치
        /// </summary>
        public void InstallTower(Vector3 position)
        {
            // TODO: ResourceManager로 실제 코인 검사
            bool isEnough = true;
            if (!isEnough) return;

            var newTowerGo = Instantiate(towerPrefab, position, Quaternion.identity, transform);
            var tower = newTowerGo.GetComponent<Tower>();
            if (tower == null)
            {
                Debug.LogError("Tower prefab에 Tower 컴포넌트가 없습니다!");
                Destroy(newTowerGo);
                return;
            }

            // Awake()에서 ID가 이미 할당됨
            towerDict.Add(tower.ID, newTowerGo);
            Debug.Log($"타워 설치 완료! ID = {tower.ID}");
        }

        /// <summary>
        /// 해당 타워를 판매(제거)하고, 코인 환급
        /// </summary>
        public void SellTower(GameObject toDelete)
        {
            var tower = toDelete.GetComponent<Tower>();
            if (tower == null || !towerDict.ContainsKey(tower.ID))
            {
                Debug.LogWarning("존재하지 않는 타워이거나 이미 판매된 타워입니다.");
                return;
            }

            // 코인 환급 로직
            int refund = tower.GetSellPrice();
            // ResourceManager.Instance.AddCoins(refund);

            towerDict.Remove(tower.ID);
            Destroy(toDelete);
            Debug.Log($"타워(ID={tower.ID}) 판매 완료! 환급 코인: {refund}");
        }

        /// <summary>
        /// 현재 설치된 모든 타워 조회
        /// </summary>
        public IEnumerable<KeyValuePair<int, GameObject>> GetAllTowers()
        {
            return towerDict;
        }
    }
}
