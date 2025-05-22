using MyGame.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterCount : MonoBehaviour
{
    public TextMeshProUGUI countText;
    [SerializeField] private StageInfo stageInfo;
    [SerializeField] private MonsterDex monsterDex;
    private StageManager stageManager;
    private MonsterWave monsterWave;
    int count = 0;

    private void Awake()
    {
        if (stageManager == null)
        {
            stageManager = StageManager.Instance;
        }
    }

    private void Start()
    {
        //Transform parentTransform = transform.parent;
        string parentImageName = GetComponentInParent<Image>().sprite.name;
        Debug.Log(parentImageName);

        //foreach (string i in MonsterManager.monsterNames)
        //{
        //    if (parentImageName == i)
        //    {
        //        count++;
        //    }
        //}

        
        //foreach (StageMonsterEntry i in stageInfo.monsterSpawnList)
        //{
        //    if (parentImageName == monsterDex.GetEntryByID(i.monsterDataIndex).monsterName)
        //    {
        //        count++;
        //    }
        //}
        
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

    private void Update()
    {
        countText.text =  "X "+ count.ToString();
    }

}
