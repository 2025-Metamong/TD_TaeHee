using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MyGame.Objects;
using MyGame.Managers;

namespace MyGame.Managers
{
    public class MonsterManager : MonoBehaviour
    {
        public static MonsterManager Instance { get; private set; }

        [Header("Wave Settings")]
        [SerializeField] private float spawnRate = 2f;

        // 현재 스폰 대기중인 몬스터 프리팹 큐
        private Queue<GameObject> waveQueue = new Queue<GameObject>();

        // 씬 내 활성화된 몬스터들을 ID → GameObject로 관리
        private Dictionary<int, GameObject> monsterDict = new Dictionary<int, GameObject>();

        [Header("Path Settings")]
        [Tooltip("몬스터가 따라갈 웨이포인트를 담은 Transform")]
        public Transform pathHolder;

        // 테스트용: Inspector에서 지정할 수 있는 몬스터 프리팹
        [Header("Test Prefab (Optional)")]
        [SerializeField] private GameObject testMonsterPrefab;

        private float spawnTimer = 0f;
        
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                // DontDestroyOnLoad if you want to persist between scenes
                // DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        void Start()
        {
            // 테스트용으로 큐에 몬스터 두 개 넣기
            if (testMonsterPrefab != null)
            {
                waveQueue.Enqueue(testMonsterPrefab);
                waveQueue.Enqueue(testMonsterPrefab);
            }
        }

        void Update()
        {
            TrySpawnMonster();
        }

        private void TrySpawnMonster()
        {

            if (waveQueue.Count == 0) return;

            Debug.Log($"[MonsterManager] waveQueue={waveQueue.Count}, pathHolder={(pathHolder==null?"null":pathHolder.name)}");


            spawnTimer += Time.deltaTime;
            if (spawnTimer < spawnRate) return;

            // 다음 몬스터 꺼내서 인스턴스화
            var prefab = waveQueue.Dequeue();
            var go = Instantiate(prefab, transform.position, Quaternion.identity, transform);

            // 고유 ID 할당된 Monster 컴포넌트 가져오기
            var monster = go.GetComponent<Monster>();
            if (monster == null)
            {
                Debug.LogError("[MonsterManager] 몬스터 프리팹에 Monster 컴포넌트가 없습니다!");
                Destroy(go);
            }
            else
            {
                // 몬스터 경로 설정
                monster.SetPath(pathHolder);

                // 딕셔너리에 등록
                monsterDict.Add(monster.ID, go);

                // UI에 남은 몬스터 수 표시 (필요시)
                UIManager.Instance.UpdateRemainingEnemies(monsterDict.Count);
            }

            spawnTimer = 0f;
        }

        /// <summary>
        /// 몬스터가 제거될 때 호출
        /// </summary>
        public void KillMonster(GameObject monsterGo)
        {
            var monster = monsterGo.GetComponent<Monster>();
            if (monster == null)
            {
                Debug.LogWarning("[MonsterManager] 제거할 객체에 Monster 컴포넌트가 없습니다.");
                return;
            }

            if (!monsterDict.ContainsKey(monster.ID))
            {
                Debug.LogWarning($"[MonsterManager] ID={monster.ID} 몬스터가 이미 제거되었거나 존재하지 않습니다.");
                return;
            }

            // 보상 코인 지급
            ResourceManager.Instance.AddCoins(monster.Reward);
            // UI에 코인 수 갱신은 ResourceManager 내부에서 처리

            // 딕셔너리에서 제거 후 오브젝트 파괴
            monsterDict.Remove(monster.ID);
            Destroy(monsterGo);

            // 남은 몬스터 수 UI 갱신
            UIManager.Instance.UpdateRemainingEnemies(monsterDict.Count);
        }

        /// <summary>
        /// 새로운 웨이브 데이터를 설정합니다.
        /// </summary>
        public void SetWave(List<GameObject> waveData)
        {
            waveQueue.Clear();
            foreach (var prefab in waveData)
                waveQueue.Enqueue(prefab);
        }

        /// <summary>
        /// 현재 씬에 남아있는 몬스터 목록(Key=ID, Value=GameObject) 열거자 반환
        /// </summary>
        public IEnumerable<KeyValuePair<int, GameObject>> GetMonsterList()
        {
            return monsterDict;
        }

        void OnDrawGizmos()
        {
            if (pathHolder == null) return;

            Vector3 prev = pathHolder.GetChild(0).position;
            foreach (Transform wp in pathHolder)
            {
                Gizmos.DrawSphere(wp.position, 0.3f);
                Gizmos.DrawLine(prev, wp.position);
                prev = wp.position;
            }
        }
    }
}
