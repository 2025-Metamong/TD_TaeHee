using MyGame.Managers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MonsterInfoPanel : MonoBehaviour
{
    public static MonsterInfoPanel Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public GameObject monsterInfoBtn;
    public GameObject imageTemplate;
    [SerializeField] private MonsterDex monsterDex;
    public Dictionary<int, int> monsterCountDict = new Dictionary<int, int>();

    private Button XBtn;
    //Transform contentPanel;

    private int checkMulti = 0;
    void Start()
    {
        XBtn = GetComponentInChildren<Button>();
        XBtn.onClick.AddListener(XBtnClicked);

        //Transform contentPanel = this.transform;
        MonsterImageSet();
    }

    public void MonsterImageSet()
    {
        List<MonsterEntry> monsterList = monsterDex.GetAllEntries();
        monsterCountDict = new Dictionary<int, int>(); // ( index,count)
        for (int i = 0; i < monsterList.Count; i++)
        {
            monsterCountDict.Add(monsterList[i].id, 0);
        }
        // existed image delete
        Image[] images = GetComponentsInChildren<Image>();

        foreach (Image image in images)
        {
            if (image.gameObject.name.StartsWith("Monster"))
            {
                //Debug.Log(image.gameObject.name);
                Debug.Log("MonsterInfoPanel : DeleteImage");
                Destroy(image.gameObject); //Monster image firnd and delete
            }
        }

        //Debug.Log("MonsterInfoPanel : " + string.Join(" , ", MonsterManager.Instance.waveMonster));

        //show monster image
        foreach (int imageName in MonsterManager.Instance.waveMonster)
        {
            foreach (var key in monsterCountDict.Keys.ToList())
            {
                if (imageName == key)
                {
                    monsterCountDict[key]++;
                }
            }
        }
        foreach (var pair in monsterCountDict)
        {
            if (pair.Value > 0)
            {
                Debug.Log("MonsterInfoPanel : ");
                instiateImage(pair.Key);
            }
        }
    }

    void instiateImage(int imageName)
    {
        Sprite sprite = Resources.Load<Sprite>($"Image/Monster/{imageName}");

        if (sprite != null)
        {
            //Debug.Log("MonsterInfoPanel : ");
            GameObject newImage = Instantiate(imageTemplate, this.transform);
            newImage.GetComponent<Image>().sprite = sprite;
            newImage.SetActive(true);
        }
        else
        {
            Debug.LogWarning($"[ImageListManager] '{imageName}' 이미지가 Resources/Image 폴더에 없습니다.");
        }
    }

    void XBtnClicked()
    {
        this.gameObject.SetActive(false);
        monsterInfoBtn.SetActive(true);
    }
}
