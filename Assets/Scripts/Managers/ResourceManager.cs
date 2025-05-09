//using UnityEngine;

//public class ResourceManager : MonoBehaviour
//{
//    public static ResourceManager Instance { get; private set; }

//    [Header("Player Resources")]
//    [SerializeField] private int hp = 10; // 플레이어 현재 체력
//    [SerializeField] private int coin = 0; // 플레이어가 소지한 돈
 
//    private void Awake()
//    {
//        if (Instance == null)
//        {
//            Instance = this;
//            DontDestroyOnLoad(gameObject);
//        }
//        else
//        {
//            Destroy(gameObject);
//        }
//    }

//    public int getCoin() => coin; // 현재 코인 반환
//    public int getHp() => hp; // 현재 체력 반환


//    public void addCoins(int amount)
//    {
//        // 몬스터 처치 시 돈 획득
//        coin += amount;
//        Debug.Log($"코인 +{amount}, 현재: {coin}");
//        UIManager.Instance.UpdateCoinUI(coin);
//    }
    
//    public bool useCoins(int amount)
//    {
//        // 타워 설치 시 돈 감소 -> return 0: 실패 / 1: 성공
//        // 호출자에서는 if(!useCoins(100)){ 실패 시 예외처리
//        if (coin >= amount)
//        {
//            coin -= amount;
//            Debug.Log($"코인 -{amount}, 현재: {coin}");
//            UIManager.Instance.UpdateCoinUI(coin);
//            return true;
//        }
//        else
//        {
//            Debug.LogWarning("코인이 부족합니다");
//            return false;
//        }
//    }

//    public void takeDamage(int damage)
//    {
//        // 몬스터 도착 시 체력 감소
//        // 데미지 후 체력이 0이면 gameOver 호출
//        hp -= damage;
//        Debug.Log($"데미지 {damage}, 남은 체력: {hp}");
//        UIManager.Instance.UpdateHPUI(hp);

//        if (hp <= 0)
//        {
//            hp = 0;
//            Debug.Log("플레이어 체력 0 -> 게임 오버");
//            StageManager.Instance.gameOver();
//        }
//    }
//}
