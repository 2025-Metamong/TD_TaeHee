using UnityEngine;

namespace MyGame.Managers
{
    public class ResourceManager : MonoBehaviour
    {
        public static ResourceManager Instance { get; private set; }

        private int coins = 0;
        private int lives = 3;

        // 외부에서 읽기만 가능한 프로퍼티
        public int CurrentCoins => coins;
        public int CurrentLives => lives;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// 코인을 사용합니다. 사용 가능하면 true, 부족하면 false.
        /// </summary>
        public bool UseCoins(int amount)
        {
            if (coins < amount) return false;
            coins -= amount;
            UIManager.Instance.UpdateCoins(coins);
            return true;
        }

        /// <summary>
        /// 코인을 추가합니다.
        /// </summary>
        public void AddCoins(int amount)
        {
            coins += amount;
            UIManager.Instance.UpdateCoins(coins);
        }

        /// <summary>
        /// 목숨(라이프)을 잃습니다.
        /// </summary>
        public void LoseLife(int count = 1)
        {
            lives = Mathf.Max(0, lives - count);
            UIManager.Instance.UpdateLives(lives);
            if (lives == 0)
                UIManager.Instance.ShowGameOver();
        }

        /// <summary>
        /// 목숨(라이프)을 추가합니다.
        /// </summary>
        public void AddLife(int count = 1)
        {
            lives += count;
            UIManager.Instance.UpdateLives(lives);
        }

        /// <summary>
        /// 코인·라이프를 초기값으로 리셋합니다.
        /// </summary>
        public void ResetResources(int startCoins, int startLives)
        {
            coins = startCoins;
            lives = startLives;
            UIManager.Instance.UpdateCoins(coins);
            UIManager.Instance.UpdateLives(lives);
        }
    }
}
