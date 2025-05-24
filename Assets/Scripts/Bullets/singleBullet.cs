using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

namespace MyGame.Objects
{
    public class singleBullet : MonoBehaviour
    {
        private Vector3 spawnPosition;
        private Vector3 direction;
        [SerializeField, Tooltip("총알이 몬스터에 주는 데미지")]
        private float damage = 10f;
        [SerializeField, Tooltip("총알 발사 속도")]
        private float speed = 50f;
        [SerializeField, Tooltip("총알 사거리")]
        private float range = 20f;

        [SerializeField, Tooltip("충돌 시 디버프 리스트")]
        private List<debuffBase> debuffList = new List<debuffBase>();
        
        // public event Action<GameObject> OnCollider; // 충돌 시 이벤트 발생 (예: 이펙트용)
        public GameObject hitEffect = null;

        public void SetDirection(Transform tower, Transform target)
        {
            Vector3 dir = (target.position - tower.position).normalized;
            this.direction = dir;
        }

        public void SetRange(float R) => this.range = R;

        public void SetDebuff(List<debuffBase> towerDebuffList) => debuffList = towerDebuffList;

        public void SetDamage(float towerDamage) => damage = towerDamage;

        void Start()
        {
            Debug.Log("Bullet Spawned");
            this.spawnPosition = transform.position;
        }

        void Update()
        {
            transform.position += this.direction * this.speed * Time.deltaTime;
            float distance = Vector3.Distance(spawnPosition, transform.position);
            if (distance > this.range) Destroy(gameObject);
        }

        void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Monster")) return;

            var target = other.GetComponent<Monster>();
            var damageMethod = target?.GetType().GetMethod("TakeDamage", new Type[] { typeof(float) });
            damageMethod?.Invoke(target, new object[] { this.damage });
            foreach (var debuff in debuffList)
                debuff.Apply(target.gameObject);

            if (hitEffect != null)
            {
                GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
                var ps = effect.GetComponent<ParticleSystem>();
                float life = ps.main.duration + ps.main.startLifetime.constantMax;
                Destroy(effect, life);
            }

            Destroy(gameObject);
        }
    }

}
