using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGame.Managers; // Assuming you have a MonsterManager script to handle monster logic

namespace MyGame.Objects
{
    public class Monster : MonoBehaviour
    {
        [Header("Monster Stats")]
        [SerializeField] private int health = 100;
        [SerializeField] private int reward = 10;
        [SerializeField] private int damage = 1;
        [SerializeField] private float speed = 1f;

        private Transform pathHolder;

        public void SetPath(Transform ways)
        {
            pathHolder = ways;
        }
        // 몬스터 이동
        private void Start()
        {
            Vector3[] waypoints = new Vector3[pathHolder.childCount];

            Debug.Log("몬스터가 소환되었습니다.");
            for (int i = 0; i < waypoints.Length; i++)
            {
                waypoints[i] = pathHolder.GetChild(i).position;
            }

            transform.position = waypoints[0];

            StartCoroutine(FollowPath(waypoints));
        }
        IEnumerator FollowPath(Vector3[] waypoints)
        {
            transform.position = waypoints[0];

            int currentWaypointIndex = 0;


            while (currentWaypointIndex < waypoints.Length - 1)
            {
                Vector3 targetWaypoint = waypoints[currentWaypointIndex + 1];
                transform.LookAt(targetWaypoint);
                transform.position = Vector3.MoveTowards(transform.position, targetWaypoint, speed * Time.deltaTime);

                if (Vector3.Distance(transform.position, targetWaypoint) < 0.01f)
                {
                    currentWaypointIndex++;
                }
                yield return null;

            }
        }
        // 몬스터 이동

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        public int GetReward()
        {
            return reward;
        }

        public void TakeDamage(int amount)
        {
            health -= amount;

            if (health <= 0f)
            {
                MonsterManager.Instance.KillMonster(this.gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other != null && other.CompareTag("Finish"))
            {
                //StageManager.Instance.ReachFinish(this);
                Debug.Log("몬스터가 Finish에 도착했습니다.");
                MonsterManager.Instance.KillMonster(this.gameObject);
            }
        }

    }

}
