using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using MyGame.Objects;

namespace MyGame.Managers
{
    public class MonsterManager : MonoBehaviour
    {
        public static MonsterManager Instance { get; private set; }

        [Header("Wave Settings")]
        [SerializeField] private float spawnRate = 2f;

        private List<GameObject> monsterList = new List<GameObject>();
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
        }

        public static int Hp = 100;
        public static int coin = 200;

        // for testing

        private float spawnTimer = 1f;

        void Update()
        {
            RespawnMonster();
        }

        // ���� ��ȯ (spawnRate�� ���� �� ������ ��ȯ)
        private void RespawnMonster()
        {
            if (waveMonster.Count == 0) return;

            spawnTimer += Time.deltaTime;
            if (spawnTimer >= spawnRate)
            {
                GameObject monsterPrefab = waveMonster.Dequeue();
                GameObject newMonster = Instantiate(monsterPrefab, transform.position, Quaternion.identity);

                // Monster���� way ����
                Monster monsterScript = newMonster.GetComponent<Monster>();
                monsterScript.SetPath(pathHolder);

                monsterList.Add(newMonster);

                spawnTimer = 0f;
            }
        }

        // ���Ͱ� �׾��� �� ȣ��
        public void KillMonster(GameObject monster)
        {
            if (monsterList.Contains(monster))
            {
                //Monster monsters1 = monster.GetComponent<Monster>();
                //ResourceManager.Instance.addCoins(monsters1.GetReward());
                monsterList.Remove(monster);
                Destroy(monster);
            }
        }

        // ���̺� ����
        public void SetWave(List<GameObject> waveData)
        {
            waveMonster.Clear();
            foreach (var monster in waveData)
            {
                waveMonster.Enqueue(monster);
            }
        }

        // ���� ��ȯ�� ���� ����Ʈ ��ȯ
        public List<GameObject> GetMonsterList()
        {
            return monsterList;
        }

        // waypoints�� �׸��� ���� Gizmos
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



