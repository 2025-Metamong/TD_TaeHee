using UnityEngine;

public class BossAttack : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Tower")
        {
            other.gameObject.SetActive(false);
        }
    }
}
