using MyGame.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterCount : MonoBehaviour
{
    //public static MonsterCount Instance { get; private set; }
    //private void Awake()
    //{
    //    if (Instance == null)
    //    {
    //        Instance = this;
    //    }
    //    else
    //    {
    //        Destroy(gameObject);
    //    }
    //}

    public TextMeshProUGUI countText;
    [SerializeField] private StageInfo stageInfo;
    [SerializeField] private MonsterDex monsterDex;
    //private StageManager stageManager;
    private MonsterWave monsterWave;
    int count = 0;
    string parentImageName;
    //bool flag = false;

    private void Start()
    {
        parentImageName = GetComponentInParent<Image>().sprite.name;
        countText = GetComponent<TextMeshProUGUI>();
        foreach(var pair in MonsterInfoPanel.Instance.monsterCountDict)
        {
            if(parentImageName == pair.Key.ToString())
            {
                count = pair.Value;
            }
        }
        countText.text = "X " + count.ToString();
    }

    //private void Update()
    //{
        
    //    if (StageManager.Instance.currentWave > buffwave)
    //    {
    //        buffwave = StageManager.Instance.currentWave;
    //        count = 0;
    //        this.UpdateMonsterCount();
    //    }
        
    //}

    private void UpdateMonsterCount()
    {
        
        //Debug.Log(parentImageName);
        monsterWave = stageInfo.monsterSpawnList[StageManager.Instance.currentWave];
        foreach (StageMonsterEntry i in monsterWave.entries)
        {
            if (parentImageName == monsterDex.GetEntryByID(i.monsterDataIndex).monsterName)
            {
                count++;
            }
        }

    }

    //public void getStageIndex(int val)
    //{
    //    stageIndex = val;
    //}
}
