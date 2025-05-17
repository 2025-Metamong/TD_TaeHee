using System.Collections.Generic;
using UnityEngine;
using MyGame.Managers;

namespace MyGame.Objects
{
    public class Tower : MonoBehaviour
    {
        // 고유 ID 자동 할당을 위한 스태틱 카운터
        private static int _nextID = 0;
        public int ID { get; private set; }

        [Header("Tower Stats")]
        [SerializeField, Tooltip("설치 비용")] private int cost = 10;
        [SerializeField, Tooltip("사거리")] private float range = 10f;
        [SerializeField, Tooltip("공격 빈도")] private float attackPeriod = 1f;
        [SerializeField, Tooltip("업그레이드 비용")] private int upgradeCost = 5;
        [SerializeField, Tooltip("탄환 Prefab")] private GameObject bulletPrefab;
        [SerializeField, Tooltip("타워 디버프 종류")] private List<debuffBase> debuffAssets = new List<debuffBase>();

        private List<debuffBase> debuffList;
        private Transform target;
        private float attackTimer = 0f;
        private Transform towerTransform;

        void Awake()
        {
            // ID 한 번만 할당
            ID = _nextID++;
            towerTransform = transform;
        }

        void Start()
        {
            // 디버프 리스트 복제
            debuffList = new List<debuffBase>(debuffAssets);
        }

        void Update()
        {
            attackTimer += Time.deltaTime;
            if (attackTimer < attackPeriod) return;
            attackTimer = 0f;

            // 사거리 내 가장 가까운 몬스터 찾기
            target = FindTarget();
            if (target == null) return;

            // 탄환 발사
            var bulletGO = Instantiate(bulletPrefab, towerTransform.position, Quaternion.identity);
            var bulletComp = bulletGO.GetComponent<MonoBehaviour>();
            // 방향 세팅
            var setDir = bulletComp?.GetType().GetMethod("SetDirection", new[] { typeof(Transform), typeof(Transform) });
            setDir?.Invoke(bulletComp, new object[] { towerTransform, target });
            // 디버프 세팅
            var setDeb = bulletComp?.GetType().GetMethod("SetDebuff", new[] { typeof(List<debuffBase>) });
            setDeb?.Invoke(bulletComp, new object[] { debuffList });
        }

        private Transform FindTarget()
        {
            float minDist = float.MaxValue;
            Transform closest = null;
            foreach (var kv in MonsterManager.Instance.GetMonsterList())
            {
                var mPos = kv.Value.transform.position;
                float dist = Vector3.Distance(towerTransform.position, mPos);
                if (dist <= range && dist < minDist)
                {
                    minDist = dist;
                    closest = kv.Value.transform;
                }
            }
            return closest;
        }

        public int GetCost() => cost;
        public int GetUpgradeCost() => upgradeCost;
        public int GetLevel() => upgradeCost; // 필요하다면 따로 레벨 필드 관리
        public int GetSellPrice() => cost / 2;
    }
}
