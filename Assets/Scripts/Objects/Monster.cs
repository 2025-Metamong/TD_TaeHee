using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyGame.Managers;

namespace MyGame.Objects
{
    public class Monster : MonoBehaviour
    {
        [Header("Monster Stats")]
        [SerializeField] private float health = 100f;
        [SerializeField] private int reward = 10;
        [SerializeField] private float speed = 1f;
        [SerializeField] private bool isStunned = false;
        [SerializeField] private float damageAmplify = 1.0f;

        // 디버프 관련
        private Transform pathHolder;
        private Dictionary<string, ActiveDebuffInfo> activeDebuffs = new Dictionary<string, ActiveDebuffInfo>();

        // 고유 ID 자동 할당
        private static int _nextID = 0;
        public int ID { get; private set; }

        // 외부에서 보상금액을 읽을 수 있도록 프로퍼티 추가
        public int Reward => reward;

        private class ActiveDebuffInfo
        {
            public Coroutine coroutine;
            public System.Action onEndAction;
            public ActiveDebuffInfo(Coroutine co, System.Action endAction)
            {
                coroutine = co;
                onEndAction = endAction;
            }
        }

        // <summary>
        // 몬스터가 사망했는지 여부를 외부에서 읽을 수 있도록 프로퍼티 추가
        // </summary>
        public bool IsDead => health <= 0f;
        /// <summary>
        /// 스턴(기절) 상태를 설정합니다.
        /// </summary>
        public void SetStun(bool tf)
        {
            isStunned = tf;
        }

        /// <summary>
        /// 현재 스턴 상태를 외부에서 읽을 수 있도록 프로퍼티도 추가하면 좋습니다.
        /// </summary>
        public bool IsStunned => isStunned;

        /// <summary>
        /// 현재 이동 속도를 반환합니다.
        /// </summary>
        public float GetSpeed()
        {
            return speed;
        }

        /// <summary>
        /// 이동 속도를 설정합니다.
        /// </summary>
        public void SetSpeed(float newSpeed)
        {
            speed = newSpeed;
        }

        /// <summary>
        /// 현재 데미지 배율을 반환합니다.
        /// </summary>
        public float GetDamageAmplify()
        {
            return damageAmplify;
        }

        /// <summary>
        /// 데미지 배율을 설정합니다.
        /// </summary>
        public void SetDamageAmplify(float value)
        {
            damageAmplify = value;
        }

        void Awake()
        {
            ID = _nextID++;
        }

        public void SetPath(Transform ways)
        {
            pathHolder = ways;
        }

        private void Start()
        {
            // 경로 초기화
            Vector3[] waypoints = new Vector3[pathHolder.childCount];
            for (int i = 0; i < waypoints.Length; i++)
                waypoints[i] = pathHolder.GetChild(i).position;

            transform.position = waypoints[0];
            StartCoroutine(FollowPath(waypoints));
        }

        IEnumerator FollowPath(Vector3[] waypoints)
        {
            int idx = 0;
            while (idx < waypoints.Length - 1)
            {
                if (activeDebuffs.ContainsKey("Stun"))
                {
                    yield return null;
                    continue;
                }

                Vector3 target = waypoints[idx + 1];
                transform.LookAt(target);
                transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

                if (Vector3.Distance(transform.position, target) < 0.01f)
                    idx++;

                yield return null;
            }
        }

        public void TakeDamage(float amount)
        {
            int finalDamage = Mathf.RoundToInt(amount * (activeDebuffs.ContainsKey("Weak") ? 1.2f : 1f));
            health -= finalDamage;
            if (health <= 0f)
            {
                MonsterManager.Instance.KillMonster(gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Finish"))
            {
                MonsterManager.Instance.KillMonster(gameObject);
            }
        }

        // 디버프 API
        public void ApplyDebuff(string key, IEnumerator debuffCoroutine, System.Action onEnd)
        {
             if (activeDebuffs.ContainsKey(key))
            {
                 activeDebuffs[key].onEndAction?.Invoke();
                StopCoroutine(activeDebuffs[key].coroutine);
                activeDebuffs.Remove(key);
            }
            Coroutine co = StartCoroutine(debuffCoroutine);
            activeDebuffs[key] = new ActiveDebuffInfo(co, onEnd);
        }

        /// <summary>
       /// 해당 키의 디버프를 중단하고 제거합니다.
       /// </summary>
        public void RemoveDebuff(string key)
       {
           if (activeDebuffs.ContainsKey(key))
           {
               // 코루틴을 멈추고
               StopCoroutine(activeDebuffs[key].coroutine);
               // 종료 콜백 실행
               activeDebuffs[key].onEndAction?.Invoke();
               // 딕셔너리에서 제거
               activeDebuffs.Remove(key);
           }
       }
        public void ClearAllDebuffs()
        {
            foreach (var info in activeDebuffs.Values)
                StopCoroutine(info.coroutine);
            activeDebuffs.Clear();
        }
    }
}
