using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 타워 관련 사운드를 관리하는 싱글톤 컨트롤러.
/// AudioClips를 에디터에서 할당한 뒤, 런타임에 PlaySound로 재생.
/// </summary>
public class TowerSoundController : MonoBehaviour
{
    public static TowerSoundController Instance { get; private set; }

    [Header("타워 사운드 클립 매핑")]
    [SerializeField, Tooltip("타워 사운드 타입별 오디오 클립 리스트")]
    private List<TowerSoundEntry> soundEntries = new List<TowerSoundEntry>();
    private Dictionary<TowerSoundType, AudioClip> soundClips;   // 내부에서 사용할 딕셔너리.
    private AudioSource audioSource;

    // 에디터에서 사운드 타입과 클립을 매핑하기 위한 구조체
    [System.Serializable]
    private struct TowerSoundEntry
    {
        public TowerSoundType type;
        public AudioClip clip;
    }

    // 사운드 종류를 정의
    public enum TowerSoundType
    {
        Install,
        Upgrade,
        Sell,
        Fail
    }

    void Awake()
    {
        // 싱글톤
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // 이걸 할까 말까..
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // 딕셔너리 초기화
        soundClips = new Dictionary<TowerSoundType, AudioClip>();
        foreach (var entry in soundEntries) // 인스펙터에서 설정한 리스트 순회.
        {
            if (entry.clip != null && !soundClips.ContainsKey(entry.type))
                soundClips.Add(entry.type, entry.clip);
        }

        audioSource = GetComponent<AudioSource>();  // 오디오 소스 컴포넌트 찾기.
    }

    /// <summary>
    /// 특정 타워 사운드를 재생. 내부에서 사용할 함수.
    /// </summary>
    private void PlaySound(TowerSoundType type)
    {
        if (!soundClips.TryGetValue(type, out AudioClip clip))
        {
            Debug.LogWarning($"[TowerSound] AudioClip이 할당되지 않았습니다: {type}");
            return;
        }

        // 일반 사운드로 재생.
        audioSource.PlayOneShot(clip);
    }

    public void PlayUpgradeSound()
    {
        PlaySound(TowerSoundType.Upgrade);
    }
    public void PlaySellSound()
    {
        PlaySound(TowerSoundType.Sell);
    }
    public void PlayInstallSound()
    {
        PlaySound(TowerSoundType.Install);

    }
    public void PlayFailedSound()
    {
        PlaySound(TowerSoundType.Fail);
    }
}
