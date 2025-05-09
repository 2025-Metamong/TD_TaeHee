//using UnityEngine;

//public class ResourceManager : MonoBehaviour
//{
//    public static ResourceManager Instance { get; private set; }

//    [Header("Player Resources")]
//    [SerializeField] private int hp = 10; // �÷��̾� ���� ü��
//    [SerializeField] private int coin = 0; // �÷��̾ ������ ��
 
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

//    public int getCoin() => coin; // ���� ���� ��ȯ
//    public int getHp() => hp; // ���� ü�� ��ȯ


//    public void addCoins(int amount)
//    {
//        // ���� óġ �� �� ȹ��
//        coin += amount;
//        Debug.Log($"���� +{amount}, ����: {coin}");
//        UIManager.Instance.UpdateCoinUI(coin);
//    }
    
//    public bool useCoins(int amount)
//    {
//        // Ÿ�� ��ġ �� �� ���� -> return 0: ���� / 1: ����
//        // ȣ���ڿ����� if(!useCoins(100)){ ���� �� ����ó��
//        if (coin >= amount)
//        {
//            coin -= amount;
//            Debug.Log($"���� -{amount}, ����: {coin}");
//            UIManager.Instance.UpdateCoinUI(coin);
//            return true;
//        }
//        else
//        {
//            Debug.LogWarning("������ �����մϴ�");
//            return false;
//        }
//    }

//    public void takeDamage(int damage)
//    {
//        // ���� ���� �� ü�� ����
//        // ������ �� ü���� 0�̸� gameOver ȣ��
//        hp -= damage;
//        Debug.Log($"������ {damage}, ���� ü��: {hp}");
//        UIManager.Instance.UpdateHPUI(hp);

//        if (hp <= 0)
//        {
//            hp = 0;
//            Debug.Log("�÷��̾� ü�� 0 -> ���� ����");
//            StageManager.Instance.gameOver();
//        }
//    }
//}
