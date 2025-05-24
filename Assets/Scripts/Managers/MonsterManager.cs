using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using MyGame.Objects;
using System;

namespace MyGame.Managers
{
    public class MonsterManager : MonoBehaviour
    {
        public static MonsterManager Instance { get; private set; }

        // private List<GameObject> monsterList = new List<GameObject>();
        // change monsterList to monsterDict
        private Dictionary<int, GameObject> monsterDict = new Dictionary<int, GameObject>();
        //private Queue<GameObject> waveMonster = new Queue<GameObject>();

        // 0520-monster dict
        //[Header("Monster List")]
        [SerializeField] private MonsterDex monsterDex;
        private int listCount = 0;
        private Queue<GameObject> respwanMonsterQueue = new Queue<GameObject>();

        private float spawnTimer = 0f;

        //0521 - stage info
        [SerializeField] private StageInfo stageInfo;

        public List<int> waveMonster = new List<int>();
        private float currentSpawnRate = 0f;
        private int monsterCount = 0;

        private Transform pathHolder; // waypoints

        //0521 - wave start
        private bool waveStartCheck = false;
        private bool waveEndCheck = false;
        private int waveIndex = 0;
        [SerializeField] private StageManager stageManager;

        // 0520 - for rouglike
        private float healthPercentDecrease = 1f;
        private float healthFlatDecrease = 0f;

        private float speedPercentDecrease = 1f;
        private float speedFlatDecrease = 0f;

        private int extraCoin = 0;
        // 0520 - for rouglike


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
            DontDestroyOnLoad(gameObject);
        }

        // for testing
        //[SerializeField] private GameObject capsulePrefab;
        //private void Start()
        //{
        //    waveMonster.Enqueue(capsulePrefab);
        //    waveMonster.Enqueue(capsulePrefab);
        //    Debug.Log($"몬스터 큐에 있는 몬스터 수 : {waveMonster.Count}");
        //}
        //[Header("Wave Settings")]
        //[SerializeField] private float spawnRate = 2f;


        //public static int Hp = 100; // for user
        //public static int coin = 200; // for user

        public static string[] monsterNames = { "Monster", "Monster2" }; ////////////////////////////////////// need modify

        // for testing

        private void Start()
        {
            List<MonsterEntry> monsterList = monsterDex.GetAllEntries();
        }

        void Update()
        {
            List<MonsterEntry> monsterList = monsterDex.GetAllEntries();
            if (waveStartCheck)
            {
                //Debug.Log($"Monster Manager : RespawnMonster, current Wave : {stageManager.currentWave}");
                RespawnMonster();
            }
            if (waveStartCheck && waveEndCheck && monsterDict.Count == 0)
            {
                waveStartCheck = false;
                waveEndCheck = false;
                stageManager.FinishWave();
                waveMonster.Clear();
                monsterDict.Clear();
                SetWave(monsterList);
                MonsterInfoPanel.Instance.MonsterImageSet();
            }

        }
        public void AllClear()
        {
            List<MonsterEntry> monsterList = monsterDex.GetAllEntries();

            waveStartCheck = false;
            waveEndCheck = false;
            waveMonster.Clear();
            respwanMonsterQueue.Clear();
            monsterDict.Clear();
            //SetWave(monsterList);
            //MonsterInfoPanel.Instance.MonsterImageSet();
        }

        private void RespawnMonster()
        {
            if (respwanMonsterQueue.Count == 0)
            {
                waveEndCheck = true; // To check if the monster wave has ended, use monsterCount.
                return;
            }

            spawnTimer += Time.deltaTime;
            if (spawnTimer >= currentSpawnRate)
            {
                int monsterID = listCount;
                GameObject monsterPrefab = respwanMonsterQueue.Dequeue();

                //GameObject newMonster = Instantiate(monsterPrefab, transform.position, Quaternion.identity);
                GameObject newMonster = Instantiate(monsterPrefab, stageInfo.spawnPoint.transform.position, Quaternion.identity);

                // Monster way set
                Monster monsterScript = newMonster.GetComponent<Monster>();
                monsterScript.SetPath(pathHolder);
                monsterScript.SetID(monsterID);
                // monsterList.Add(newMonster);
                // Change monsterList to monsterDict
                monsterDict.Add(monsterID, newMonster);

                // 0520-rouglike
                // health down
                float newHp = monsterScript.GetHealth() * healthPercentDecrease - healthFlatDecrease;
                monsterScript.SetHealth(Mathf.Max(1f, newHp));
                //Debug.Log($"{newHp} {healthPercentDecrease} {healthFlatDecrease}");

                // speed down
                float newSpeed = monsterScript.GetSpeed() * speedPercentDecrease - speedFlatDecrease;
                monsterScript.SetSpeed(Mathf.Max(0.1f, newSpeed));
                //Debug.Log($"{newSpeed} {speedPercentDecrease} {speedFlatDecrease}");

                // reward up
                monsterScript.SetReward(extraCoin);

                spawnTimer = 0f;
                listCount++;
                monsterCount++;


                if (monsterCount < stageInfo.monsterSpawnList[stageManager.currentWave].entries.Count)
                {
                    currentSpawnRate = stageInfo.monsterSpawnList[stageManager.currentWave].entries[monsterCount].spawnTime;
                }
            }
        }

        // 0520 - for rouglike
        public void SetHealthDecrease(float percent, float flatAmount)
        {
            healthPercentDecrease = percent;
            healthFlatDecrease = flatAmount;
        }
        public void SetSpeedDecrease(float percent, float flatAmount)
        {
            speedPercentDecrease = percent;
            speedFlatDecrease = flatAmount;
        }
        public void SetExtraCoin(int bonus)
        {
            extraCoin = bonus;
        }

        public void ResetRoguelike()
        {
            healthPercentDecrease = 1f;
            healthFlatDecrease = 0f;
            speedPercentDecrease = 1f;
            speedFlatDecrease = 0f;
            extraCoin = 0;
        }
        // 0520 - for rouglike

        public void KillMonster(GameObject monster)
        {
            // if (monsterList.Contains(monster))
            // {
            //     //Monster monsters1 = monster.GetComponent<Monster>();
            //     //ResourceManager.Instance.addCoins(monsters1.GetReward());
            //     monsterList.Remove(monster);
            //     Destroy(monster);
            // }

            // Change monsterList to monsterDict

            var monsterScript = monster.GetComponent<MonoBehaviour>();
            var monsterID = monsterScript?.GetType()?.GetMethod("GetID", new Type[] { })?.Invoke(monsterScript, new object[] { });
            if (monsterDict.ContainsKey((int)monsterID))
            {
                //Monster monsters1 = monster.GetComponent<Monster>();
                //ResourceManager.Instance.addCoins(monsters1.GetReward());
                //audioSource.PlayOneShot(death);
                monsterDict.Remove((int)monsterID);
                Destroy(monster);
            }
        }

        public void StartWave(int val)
        {
            monsterCount = 0;
            waveIndex = val;
            waveStartCheck = true;
            //Debug.Log("Monster Manager : Start wave");
        }

        /// /////////////////////////////////// after wave idea, need modify
        public void SetWave(List<MonsterEntry> monsterList)
        {
            waveMonster.Clear();
            if (stageInfo.monsterSpawnList.Count <= stageManager.currentWave)
            {
                stageManager.FinishStage();
                GameManager.Instance.ExitScene();
                return;
            }

            // wave monster set
            foreach (var i in stageInfo.monsterSpawnList[stageManager.currentWave].entries)
            {
                waveMonster.Add(i.monsterDataIndex);
                //Debug.Log(i.monsterDataIndex);
            }

            for (int i = 0; i < waveMonster.Count; i++)
            {
                for (int j = 0; j < monsterList.Count; j++)
                {
                    if (waveMonster[i] == monsterList[j].id)
                    {
                        respwanMonsterQueue.Enqueue(monsterList[j].prefab);
                    }
                }
            }
        }

        // public List<GameObject> GetMonsterList()
        // {
        //     return monsterList;
        // }
        // Change monsterList to monsterDict
        public IEnumerable<KeyValuePair<int, GameObject>> GetMonsterList()
        {   // return Enumerator of Dictionary. each item is "KeyValuePair<int,GameObject>"
            return this.monsterDict;
        }

        // draw Gizmos for way point
        //void OnDrawGizmos()
        //{
        //    Vector3 startPosition = pathHolder.GetChild(0).position;
        //    Vector3 previousPosition = startPosition;

        //    foreach (Transform waypoint in pathHolder)
        //    {
        //        Gizmos.DrawSphere(waypoint.position, 0.3f);
        //        Gizmos.DrawLine(previousPosition, waypoint.position);
        //        previousPosition = waypoint.position;
        //    }
        //}

        // StageManager Interface
        public void SetMonsterManagerStageInfo(StageInfo si)
        {
            this.stageInfo = si;
            AllClear();

            List<MonsterEntry> monsterList = monsterDex.GetAllEntries();
            //respwanMonsterQueue.Clear();
            SetWave(monsterList);
            
            pathHolder = stageInfo.pathHolder; // waypoints set
            currentSpawnRate = stageInfo.monsterSpawnList[stageManager.currentWave].entries[0].spawnTime;
        }
    }

}