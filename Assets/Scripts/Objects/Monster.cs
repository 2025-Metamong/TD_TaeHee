using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGame.Managers; // Assuming you have a MonsterManager script to handle monster logic

namespace MyGame.Objects
{
    public class Monster : MonoBehaviour
    {
        [Header("Monster Stats")]
        [SerializeField] private float health = 100;
        [SerializeField] private int reward = 10;
        [SerializeField] private int damage = 1;
        [SerializeField] private float speed = 1f;
        // Add Monster ID 
        [SerializeField] private int ID = -1;
        private Transform pathHolder;

        // 디버프 관련 변수 추가
        public bool isDead = false;
        private class ActiveDebuffInfo
        {
            public Coroutine coroutine;
            public System.Action onEndAction;

            public ActiveDebuffInfo(Coroutine co, System.Action endAction)
            {
                this.coroutine = co;
                this.onEndAction = endAction;
            }
        }
        private Dictionary<string, ActiveDebuffInfo> activeDebuffs = new Dictionary<string, ActiveDebuffInfo>();

        // Slow Debuff
        public float GetSpeed() => speed;
        public void SetSpeed(float newSpeed) => speed = newSpeed;

        // Weak Debuff 
        [SerializeField] private float damageAmplify = 1.0f;
        public float GetDamageAmplify() => damageAmplify;
        public void SetDamageAmplify(float value) => damageAmplify = value;

        // Stun Debuff
        [SerializeField] private bool isStunned = false;
        public void SetStun(bool tf) => isStunned = tf;


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
                if (this.isStunned)
                {
                    yield return null;
                    continue;
                }
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

        // 새로운 함수 구현을 위해 디버프 관련 추가 함수 주석처리리
        // public Vector3 GetPosition()
        // {
        //     return transform.position;
        // }

        // public int GetReward()
        // {
        //     return reward;
        // }

        // public void AddReward(int value)
        // {
        //     reward += value;
        // }

        // 디버프 부여 함수
        public void ApplyDebuff(string debuffKey, IEnumerator debuffCoroutine, System.Action onEndAction)
        {
            if (activeDebuffs.ContainsKey(debuffKey))
            {
                activeDebuffs[debuffKey].onEndAction?.Invoke();
                StopCoroutine(activeDebuffs[debuffKey].coroutine);
                activeDebuffs.Remove(debuffKey);
            }

            Coroutine co = StartCoroutine(debuffCoroutine);
            activeDebuffs[debuffKey] = new ActiveDebuffInfo(co, onEndAction);
        }

        // 디버프 삭제 함수
        public void RemoveDebuff(string debuffKey)
        {
            if (activeDebuffs.ContainsKey(debuffKey))
            {
                activeDebuffs.Remove(debuffKey);
            }
        }

        // 죽으면 모든 디버프 정리
        public void ClearAllDebuffs()
        {
            foreach (var debuff in activeDebuffs.Values)
            {
                if (debuff != null)
                {
                    StopCoroutine(debuff.coroutine);
                }
            }
            activeDebuffs.Clear();
        }


        public void TakeDamage(float amount)
        {
            //health -= amount;
            int amplifiedDamage = Mathf.RoundToInt(amount * damageAmplify);
            health -= amplifiedDamage;
            Debug.Log($"Hit Monster Damage : {amplifiedDamage}");

            if (health <= 0f)
            {
                this.isDead = true;
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

        public int GetID(){
            return this.ID;
        }

        public void SetID(int id){
            this.ID = id;
        }
    }

}