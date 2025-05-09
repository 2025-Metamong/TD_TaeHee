using UnityEngine;
using System;

public class debuffBullet : MonoBehaviour
{
    private Vector3 spawnPosition;
    private Vector3 direction;
    private float damage = 10f;
    private float range = 20f;

    // public event Action<GameObject> OnCollider; // 충돌 시 이벤트 발생 (예: 이펙트용)
    public GameObject hitEffect = null;

    void Start() {
        this.spawnPosition = transform.position;
        ApplyDamageToNearbyMonsters();
        if (hitEffect != null) Instantiate(hitEffect, spawnPosition, Quaternion.identity);
        Destroy(gameObject);
    }

    void ApplyDamageToNearbyMonsters() {
        Collider[] hits = Physics.OverlapSphere(spawnPosition, range);

        foreach(var hit in hits) {
            if(hit.CompareTag("Monster")) {
                var target = hit.GetComponent<MonoBehaviour>();
                var method = target?.GetType().GetMethod("takeDamage", new Type[] { typeof(float) });
                method?.Invoke(target, new object[] { this.damage });
            }
        }
    }

    // 씬(Scene) 뷰에서 이 오브젝트를 선택했을 때만 호출되는 디버그용 콜백 함수
    void OnDrawGizmosSelected() {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
