using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyGame.Managers; // Assuming you have a MonsterManager script to handle monster logic

namespace MyGame.Objects
{
    public class Monster : MonoBehaviour
    {
        [Header("Monster Stats")]
        [SerializeField] private float health = 100;
        [SerializeField] private int reward = 10;
        [SerializeField] private int damage = 1; // monster attack damage
        [SerializeField] private float speed = 1f;
        [SerializeField] private int index = -1;
        // Add Monster ID 
        [SerializeField] private int ID = -1;
        private Transform pathHolder;
        private Dictionary<string, ActiveDebuffInfo> activeDebuffs = new Dictionary<string, ActiveDebuffInfo>();

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

        // 0520-rouglike
        public float GetHealth() => health;
        public void SetHealth(float newHealth) => health = newHealth;
        public void SetReward(int extraReward) => reward += extraReward;

        // 5023-sound
        //public AudioClip deathSound;

        // 맞았을 때 색깔 변경
        private Coroutine flashCoroutine;

        public void SetPath(Transform ways)
        {
            pathHolder = ways;
        }

        // 몬스터 이동
        private void Start()
        {
            Vector3[] waypoints = new Vector3[pathHolder.childCount];
            

            //Debug.Log("몬스터가 소환되었습니다.");
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
                AudioManager.Instance.PlaySound(index);
                //AudioSource.PlayClipAtPoint(deathSound, transform.position);
                this.isDead = true;
                MonsterManager.Instance.KillMonster(this.gameObject);
                StageManager.Instance.AddCoins(reward);
            }

            if (flashCoroutine == null)
                flashCoroutine = StartCoroutine(Flash());
        }

        private IEnumerator Flash()
        {
            // 1) 모든 Renderer
            var rends = GetComponentsInChildren<Renderer>(true);
            // 1-1) ParticleSystemRenderer (별도 처리)
            var prends = GetComponentsInChildren<ParticleSystemRenderer>(true);
            // 1-2) UI Graphic (Image, TextMeshProUGUI 등)
            var graphics = GetComponentsInChildren<Graphic>(true);

            // 저장용
            var originalMatColors = new List<Color[]>();
            var originalBlocks = new List<MaterialPropertyBlock>();

            // --- Renderer 계열 색 바꾸기 ---
            foreach (var r in rends)
            {
                // 머티리얼 슬롯 배열 복사
                var mats = r.materials;
                var cols = new Color[mats.Length];

                for (int i = 0; i < mats.Length; i++)
                {
                    // 저장
                    cols[i] = mats[i].HasProperty("_Color")
                                ? mats[i].color
                                : Color.white;
                    // 적용
                    if (mats[i].HasProperty("_Color"))
                        mats[i].color = Color.red;
                    else
                    {
                        // 셰이더에 _Color가 없으면 PropertyBlock
                        var block = new MaterialPropertyBlock();
                        r.GetPropertyBlock(block);
                        block.SetColor("_Color", Color.red);
                        r.SetPropertyBlock(block);
                    }
                }

                // 재할당 & 저장
                r.materials = mats;
                originalMatColors.Add(cols);
            }

            // --- ParticleSystemRenderer 색 바꾸기 ---
            foreach (var pr in prends)
            {
                var mats = pr.materials;
                var cols = new Color[mats.Length];
                for (int i = 0; i < mats.Length; i++)
                {
                    cols[i] = mats[i].HasProperty("_Color") ? mats[i].color : Color.white;
                    if (mats[i].HasProperty("_Color"))
                        mats[i].color = Color.red;
                }
                pr.materials = mats;
                originalMatColors.Add(cols);
            }

            // --- UI Graphic 색 바꾸기 ---
            var originalUIColors = new List<Color>();
            foreach (var g in graphics)
            {
                originalUIColors.Add(g.color);
                g.color = Color.red;
            }

            // 0.1초 대기
            yield return new WaitForSeconds(0.1f);

            // --- 복원: Renderer + ParticleSystemRenderer ---
            int matIndex = 0;
            foreach (var r in rends)
            {
                var mats = r.materials;
                var cols = originalMatColors[matIndex++];
                for (int i = 0; i < mats.Length; i++)
                    if (mats[i].HasProperty("_Color"))
                        mats[i].color = cols[i];
                r.materials = mats;
                r.SetPropertyBlock(null);
            }
            foreach (var pr in prends)
            {
                var mats = pr.materials;
                var cols = originalMatColors[matIndex++];
                for (int i = 0; i < mats.Length; i++)
                    if (mats[i].HasProperty("_Color"))
                        mats[i].color = cols[i];
                pr.materials = mats;
            }

            // --- 복원: UI Graphic ---
            for (int i = 0; i < graphics.Length; i++)
                graphics[i].color = originalUIColors[i];

            flashCoroutine = null;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other != null && other.CompareTag("Finish"))
            {
                //StageManager.Instance.ReachFinish(this);
                Debug.Log("몬스터가 Finish에 도착했습니다.");
                AudioManager.Instance.PlayerHitSound();
                StageManager.Instance.TakeDamage(damage);
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