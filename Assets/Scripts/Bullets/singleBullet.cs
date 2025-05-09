using UnityEngine;
using System;

namespace MyGame.Objects
{
    public class singleBullet : MonoBehaviour
    {
        private Vector3 spawnPosition;
        private Vector3 direction;
        private float damage = 10f;
        private float speed = 50f;
        private float range = 20f;

        // public event Action<GameObject> OnCollider; // 충돌 시 이벤트 발생 (예: 이펙트용)
        public GameObject hitEffect = null;

        public void SetDirection(Transform tower, Transform target)
        {
            Vector3 dir = (target.position - tower.position).normalized;
            this.direction = dir;
        }

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

            // Monster mon = other.GetComponent<MonoBehaviour>();
            // mon.TakeDamage(this.damage)
            Debug.Log("Hit Monster");

            object target = other.GetComponent<MonoBehaviour>();
            var method = target?.GetType().GetMethod("TakeDamage", new Type[] { typeof(float) });
            method?.Invoke(target, new object[] { this.damage });

            if (hitEffect != null) Instantiate(hitEffect, transform.position, Quaternion.identity);

            // OnCollider?.Invoke(other.gameObject);
        }
    }

}
