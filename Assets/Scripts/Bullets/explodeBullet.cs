using UnityEngine;
using System;

public class explodeBullet : MonoBehaviour
{
    private Vector3 spawnPosition;
    private Vector3 direction;
    private float damage = 10f;
    private float speed = 5f;
    private float range = 20f;
    private float radius = 3f;

    // public event Action<GameObject> OnCollider; // 충돌 시 이벤트 발생 (예: 이펙트용)
    public GameObject hitEffect = null;

    public void setDirection(Transform tower, Transform target) {
        Vector3 dir = (target.position - tower.position).normalized;
        this.direction = dir;
    }

    void Start() {
        this.spawnPosition = transform.position;
    }

    void Update() {
        transform.position += this.direction * this.speed * Time.deltaTime;
        float distance = Vector3.Distance(spawnPosition, transform.position);
        if(distance > this.range) Destroy(gameObject);
        
    }

    void OnTriggerEnter(Collider other) {
        if(!other.CompareTag("Monster")) return;

        Collider[] hits = Physics.OverlapSphere(transform.position, radius);
        if(hitEffect != null) Instantiate(hitEffect, transform.position, Quaternion.identity);

        foreach(var hit in hits) {
            if (hit.CompareTag("Monster")) {
                object target = hit.GetComponent<MonoBehaviour>();
                var method = target?.GetType().GetMethod("takeDamage", new Type[] { typeof(float) });
                method?.Invoke(target, new object[] { this.damage });
            }
        }
    }
}
