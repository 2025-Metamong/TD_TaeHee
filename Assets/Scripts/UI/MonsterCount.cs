using MyGame.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterCount : MonoBehaviour
{
    public static MonsterCount Instance { get; private set; }
    private void Awake()
    {
        if (stageManager == null)
        {
            stageManager = StageManager.Instance;
        }
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public TextMeshProUGUI countText;
    [SerializeField] private StageInfo stageInfo;
    [SerializeField] private MonsterDex monsterDex;
    private StageManager stageManager;
    private MonsterWave monsterWave;
    int count = 0;
    int buffwave = 0;
    int stageIndex = 0;

    private void Start()
    {
        this.UpdateMonsterCount();
    }

    private void Update()
    {
        countText.text = "X " + count.ToString();
        if (stageManager.currentWave > buffwave)
        {
            buffwave = stageManager.currentWave;
            count = 0;
            this.UpdateMonsterCount();
        }
        
    }

    private void UpdateMonsterCount()
    {
        string parentImageName = GetComponentInParent<Image>().sprite.name;
        Debug.Log(parentImageName);
        
        monsterWave = stageInfo.monsterSpawnList[stageManager.currentWave];
        foreach (StageMonsterEntry i in monsterWave.entries)
        {
            if (parentImageName == monsterDex.GetEntryByID(i.monsterDataIndex).monsterName)
            {
                count++;
            }
        }

        countText = GetComponent<TextMeshProUGUI>();
    }

    public void getStageIndex(int val)
    {
        stageIndex = val;
    }
}
