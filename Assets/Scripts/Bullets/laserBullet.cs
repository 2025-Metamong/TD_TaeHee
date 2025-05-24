using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

namespace MyGame.Objects
{
    public class laserBullet : MonoBehaviour
    {
        private Vector3 direction;
        private float damage = 10f;
        private float range = 20f;
        private float lifeTime = 0.2f; // 레이저 시각적 유지 시간
        
        [SerializeField, Tooltip("충돌 시 디버프 리스트")]
        private List<debuffBase> debuffList = new List<debuffBase>();

        public GameObject hitEffect;

        public void SetDirection(Vector3 dir)
        {
            this.direction = dir.normalized;
        }
        public void SetRange(float R) => this.range = R;

        public void SetDebuff(List<debuffBase> towerDebuffList) => debuffList = towerDebuffList;

        public void SetDamage(float towerDamage) => damage = towerDamage;

        void Start()
        {
            FireLaser();
            Invoke(nameof(DestroySelf), lifeTime); // 일정 시간 후 제거
        }

        void FireLaser()
        {
            // 레이저 경로 상의 모든 콜라이더 가져오기
            Ray ray = new Ray(transform.position, direction);
            RaycastHit[] hits = Physics.SphereCastAll(ray, 0.5f, range);

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.CompareTag("Monster"))
                {
                    object target = hit.collider.GetComponent<MonoBehaviour>();
                    var method = target?.GetType().GetMethod("takeDamage", new Type[] { typeof(float) });
                    method?.Invoke(target, new object[] { this.damage });

                    if (hitEffect != null) Instantiate(hitEffect, hit.point, Quaternion.identity);
                }
            }

            // 레이저 길이에 맞춰 시각적 표현 조정
            AdjustLaserVisual();
        }

        void AdjustLaserVisual()
        {
            // 레이저가 forward 방향으로 range만큼 뻗도록 스케일 조정
            if (TryGetComponent<LineRenderer>(out var lr))
            {
                lr.SetPosition(0, transform.position);
                lr.SetPosition(1, transform.position + direction * range);
            }
        }

        void DestroySelf()
        {
            Destroy(gameObject);
        }
    }
}