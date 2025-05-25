using MyGame.Managers;
using UnityEngine;
using UnityEngine.Audio;

public class BossAttack : MonoBehaviour
{
    [SerializeField]private AudioClip auidoClip;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Tower")
        {
            //other.gameObject.SetActive(false);
            audioSource.PlayOneShot(auidoClip);
            TowerManager.Instance.DestroyTower(other.gameObject);
        }
    }
}
