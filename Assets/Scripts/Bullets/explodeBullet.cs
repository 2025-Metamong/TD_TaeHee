using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

namespace MyGame.Objects
{
    public class explodeBullet : MonoBehaviour
    {
        private Vector3 spawnPosition;
        private Vector3 direction;
        private float damage = 10f;
        private float speed = 5f;
        private float range = 20f;
        private float radius = 3f;

        [SerializeField, Tooltip("충돌 시 디버프 리스트")]
        private List<debuffBase> debuffList = new List<debuffBase>();

        // public event Action<GameObject> OnCollider; // 충돌 시 이벤트 발생 (예: 이펙트용)
        public GameObject hitEffect = null;

        public void setDirection(Transform tower, Transform target)
        {
            Vector3 dir = (target.position - tower.position).normalized;
            this.direction = dir;
        }

        public void SetDebuff(List<debuffBase> towerDebuffList) => debuffList = towerDebuffList;

        public void SetDamage(float towerDamage) => damage = towerDamage;

        void Start()
        {
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

            if (hitEffect != null)
            {
                GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
                var ps = effect.GetComponent<ParticleSystem>();
                float life = ps.main.duration + ps.main.startLifetime.constantMax;
                Destroy(effect, life);
            }
            
            Collider[] hits = Physics.OverlapSphere(transform.position, radius);
            
            foreach (var hit in hits)
            {
                if (hit.CompareTag("Monster"))
                {
                    var target = hit.GetComponent<Monster>();
                    var method = target?.GetType().GetMethod("TakeDamage", new Type[] { typeof(float) });
                    method?.Invoke(target, new object[] { this.damage });
                    foreach (var debuff in debuffList)
                        debuff.Apply(target.gameObject);
                }
            }

            Destroy(gameObject);
        }
    }
}