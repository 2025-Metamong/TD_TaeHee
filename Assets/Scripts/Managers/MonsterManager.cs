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

        [Header("Wave Settings")]
        [SerializeField] private float spawnRate = 2f;

        // private List<GameObject> monsterList = new List<GameObject>();
        // change monsterList to monsterDict
        private Dictionary<int, GameObject> monsterDict = new Dictionary<int, GameObject>();
        private Queue<GameObject> waveMonster = new Queue<GameObject>();

        public Transform pathHolder; // waypoints
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

        // for testing
        [SerializeField] private GameObject capsulePrefab;
        private void Start()
        {
            waveMonster.Enqueue(capsulePrefab);
            waveMonster.Enqueue(capsulePrefab);
            Debug.Log($"몬스터 큐에 있는 몬스터 수 : {waveMonster.Count}");
        }

        public static int Hp = 100;
        public static int coin = 200;

        public static string[] monsterNames = {"Monster", "Monster2"};

        // for testing

        private float spawnTimer = 1f;

        void Update()
        {
            RespawnMonster();
        }

        // ???? ??? (spawnRate?? ???? ?? ?????? ???)
        private void RespawnMonster()
        {
            if (waveMonster.Count == 0) return;

            spawnTimer += Time.deltaTime;
            if (spawnTimer >= spawnRate)
            {
                int monsterID = waveMonster.Count;
                GameObject monsterPrefab = waveMonster.Dequeue();
                GameObject newMonster = Instantiate(monsterPrefab, transform.position, Quaternion.identity);

                // Monster???? way ????
                Monster monsterScript = newMonster.GetComponent<Monster>();
                monsterScript.SetPath(pathHolder);
                monsterScript.SetID(monsterID);
                // monsterList.Add(newMonster);
                // Change monsterList to monsterDict
                monsterDict.Add(monsterID, newMonster);

                spawnTimer = 0f;
            }
        }

        // ????? ????? ?? ???
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
            var monsterID = monsterScript?.GetType()?.GetMethod("GetID", new Type[]{})?.Invoke(monsterScript, new object[]{});
            if (monsterDict.ContainsKey((int)monsterID))
            {
                //Monster monsters1 = monster.GetComponent<Monster>();
                //ResourceManager.Instance.addCoins(monsters1.GetReward());
                
                monsterDict.Remove((int)monsterID);
                Destroy(monster);
            }
        }

        // ????? ????
        public void SetWave(List<GameObject> waveData)
        {
            waveMonster.Clear();
            foreach (var monster in waveData)
            {
                waveMonster.Enqueue(monster);
            }
        }

        // ???? ????? ???? ????? ???
        // public List<GameObject> GetMonsterList()
        // {
        //     return monsterList;
        // }
        // ???? ????? ???? ????? ???
        // Change monsterList to monsterDict
        public IEnumerable<KeyValuePair<int, GameObject>> GetMonsterList()
        {   // return Enumerator of Dictionary. each item is "KeyValuePair<int,GameObject>"
            return this.monsterDict;
        }

        // waypoints?? ????? ???? Gizmos
        void OnDrawGizmos()
        {
            Vector3 startPosition = pathHolder.GetChild(0).position;
            Vector3 previousPosition = startPosition;

            foreach (Transform waypoint in pathHolder)
            {
                Gizmos.DrawSphere(waypoint.position, 0.3f);
                Gizmos.DrawLine(previousPosition, waypoint.position);
                previousPosition = waypoint.position;
            }
        }
    }

}