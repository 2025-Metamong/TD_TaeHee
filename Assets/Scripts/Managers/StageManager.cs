using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace MyGame.Managers
{
    public class StageManager : MonoBehaviour
    {
        public static StageManager Instance { get; private set; }

        [Header("Stage Data")]
        [SerializeField] private List<WaveData> waves;   // Level 씬에서 사용
        [SerializeField, Min(1)]
        private int totalStageCount = 6;                  // SelectStage 씬에서 버튼 검증에만 사용

        public int TotalWaves => waves != null && waves.Count > 0 
                          ? waves.Count 
                          : totalStageCount;

        [Header("Initial Resources")]
        [SerializeField] private int startCoins = 200;
        [SerializeField] private int startLives = 3;

        // ← 이 두 필드를 클래스 상단에 선언해야 합니다.
        private int currentWave = 0;
        private bool stageActive = false;

        void Awake()
        {
            if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
            else { Destroy(gameObject); return; }

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name.StartsWith("Level"))
                StartStage();
        }

        public void StartStage()
        {
            stageActive = true;
            currentWave = 0;

            ResourceManager.Instance.ResetResources(startCoins, startLives);
            UIManager.Instance.UpdateCoins(ResourceManager.Instance.CurrentCoins);
            UIManager.Instance.UpdateLives(ResourceManager.Instance.CurrentLives);

            NextWave();
        }

        private void NextWave()
        {
            if (waves == null || waves.Count == 0)
            {
                Debug.LogError("[StageManager] waves 리스트가 비어있습니다!");
                StageFail();
                return;
            }

            currentWave++;
            if (currentWave > waves.Count)
            {
                StageClear();
                return;
            }

            UIManager.Instance.UpdateWave(currentWave, TotalWaves);
            MonsterManager.Instance.SetWave(waves[currentWave - 1].monsterPrefabs);
        }

        void Update()
        {
            if (!stageActive) return;

            // 몬스터가 모두 사라지면 다음 웨이브
            if (!MonsterManager.Instance.GetMonsterList().GetEnumerator().MoveNext())
                NextWave();
        }

        private void StageClear()
        {
            stageActive = false;
            UIManager.Instance.ShowStageClear();
        }

        public void StageFail()
        {
            stageActive = false;
            UIManager.Instance.ShowGameOver();
        }
    }

    [System.Serializable]
    public class WaveData
    {
        public List<GameObject> monsterPrefabs;
    }
}
