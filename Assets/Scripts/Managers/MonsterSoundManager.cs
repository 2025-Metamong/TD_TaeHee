using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource sfxSource;
    [SerializeField] private MonsterDex monsterDex;
    [SerializeField] private AudioClip plyaerDamageSound;

    void Awake()
    {
        Instance = this;
        sfxSource = GetComponent<AudioSource>();
    }

    public void PlaySound(int monsterIndex)
    {
        sfxSource.PlayOneShot(monsterDex.GetEntryByID(monsterIndex).audioClip);
    }

    public void PlayerHitSound()
    {
        sfxSource.PlayOneShot(plyaerDamageSound);
    }
}
